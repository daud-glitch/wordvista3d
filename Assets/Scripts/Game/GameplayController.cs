using System;
using System.Collections;
using UnityEngine;
using WordVista.Levels;
using WordVista.Crossword;
using WordVista.InputSystem;
using WordVista.Economy;
using WordVista.Save;
using WordVista.CameraSystem;
using WordVista.Environment;
using WordVista.Utilities;

namespace WordVista.Game
{
    public class GameplayController : MonoBehaviour
    {
        [Header("Subsystems")]
        [SerializeField] private CrosswordRenderer crosswordRenderer;
        [SerializeField] private LetterWheel letterWheel;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private EnvironmentalProgressController environmentController;

        [Header("UI Views")]
        [SerializeField] private GameObject winModal;
        [SerializeField] private TMPro.TextMeshProUGUI levelTitleText;
        [SerializeField] private TMPro.TextMeshProUGUI coinCountText;
        [SerializeField] private TMPro.TextMeshProUGUI bonusCountText;

        [Header("Audio")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip wordSolvedClip;
        [SerializeField] private AudioClip bonusWordClip;
        [SerializeField] private AudioClip levelCompleteClip;

        private LevelData _currentLevel;
        private CrosswordGridModel _gridModel;
        private WordValidator _wordValidator;
        private WordTrie _dictionary;
        private bool _usedHintThisLevel;

        public event Action<LevelData> OnLevelLoaded;
        public event Action OnLevelCompleted;

        private void Awake()
        {
            _gridModel = new CrosswordGridModel();
            _dictionary = new WordTrie();
            LoadBundledDictionary();
            _wordValidator = new WordValidator(_dictionary);

            if (letterWheel != null)
            {
                letterWheel.OnWordSubmitted += HandleWordSubmitted;
            }

            EconomyManager.Instance.OnCoinsChanged += UpdateCoinDisplay;
        }

        private void Start()
        {
            UpdateCoinDisplay(EconomyManager.Instance.Coins, 0);
            int targetLevel = SaveManager.Instance.Data.currentLevelIndex;
            LoadLevel(targetLevel);
        }

        private void OnDestroy()
        {
            if (letterWheel != null)
            {
                letterWheel.OnWordSubmitted -= HandleWordSubmitted;
            }
            EconomyManager.Instance.OnCoinsChanged -= UpdateCoinDisplay;
        }

        public void LoadLevel(int levelId)
        {
            _usedHintThisLevel = false;
            if (winModal != null) winModal.SetActive(false);

            // Load Level from Resources
            _currentLevel = LevelRepository.GetLevel(levelId);
            if (_currentLevel == null)
            {
                Debug.LogError($"[GameplayController] Level {levelId} not found! Loading Level 1 fallback.");
                _currentLevel = LevelRepository.GetLevel(1);
            }

            if (levelTitleText != null)
            {
                levelTitleText.text = $"Level {_currentLevel.levelId} • {_currentLevel.worldName}";
            }

            _gridModel.LoadLevel(_currentLevel);
            _wordValidator.SetLevel(_currentLevel);

            if (crosswordRenderer != null)
            {
                crosswordRenderer.RenderGrid(_gridModel);
            }

            if (letterWheel != null)
            {
                letterWheel.SetupWheel(_currentLevel.letters);
            }

            if (environmentController != null)
            {
                environmentController.InitializeForWorld(_currentLevel.worldId,
                    SaveManager.Instance.GetWorldProgress(_currentLevel.worldId).highestLevelCompleted);
            }

            if (cameraController != null)
            {
                cameraController.PlayIntroSequence();
            }

            OnLevelLoaded?.Invoke(_currentLevel);
        }

        private void HandleWordSubmitted(string guess)
        {
            var result = _wordValidator.ValidateWord(guess, out var placement);

            switch (result)
            {
                case WordValidationResult.MainAnswer:
                    OnMainWordSolved(guess, placement);
                    break;

                case WordValidationResult.BonusWord:
                    OnBonusWordSolved(guess);
                    break;

                case WordValidationResult.AlreadyFoundMain:
                case WordValidationResult.AlreadyFoundBonus:
                case WordValidationResult.Invalid:
                    if (letterWheel != null)
                    {
                        letterWheel.PlayInvalidFeedback();
                    }
                    break;
            }
        }

        private void OnMainWordSolved(string word, WordPlacement placement)
        {
            _gridModel.TrySolveWord(word, out _);

            if (crosswordRenderer != null)
            {
                crosswordRenderer.AnimateWordSolved(placement);
            }

            if (audioSource != null && wordSolvedClip != null)
            {
                audioSource.PlayOneShot(wordSolvedClip);
            }

            if (environmentController != null)
            {
                environmentController.OnWordSolvedReaction();
            }

            // Check if all words are solved!
            if (_gridModel.IsComplete)
            {
                StartCoroutine(CompleteLevelRoutine());
            }
        }

        private void OnBonusWordSolved(string word)
        {
            if (audioSource != null && bonusWordClip != null)
            {
                audioSource.PlayOneShot(bonusWordClip);
            }

            if (bonusCountText != null)
            {
                bonusCountText.text = $"+Bonus Word! ({SaveManager.Instance.Data.totalBonusWordsFound})";
            }
        }

        private IEnumerator CompleteLevelRoutine()
        {
            yield return new WaitForSeconds(0.7f);

            if (audioSource != null && levelCompleteClip != null)
            {
                audioSource.PlayOneShot(levelCompleteClip);
            }

            if (cameraController != null)
            {
                cameraController.PlayLevelCompleteMotion();
            }

            int reward = _currentLevel.coinReward;
            SaveManager.Instance.MarkLevelCompleted(_currentLevel.levelId, _currentLevel.worldId, reward, !_usedHintThisLevel);

            if (environmentController != null)
            {
                environmentController.CheckLevelProgress(_currentLevel.levelId);
            }

            yield return new WaitForSeconds(0.8f);

            if (winModal != null)
            {
                winModal.SetActive(true);
            }

            OnLevelCompleted?.Invoke();
        }

        public void OnNextLevelClicked()
        {
            int next = _currentLevel.levelId + 1;
            if (next <= 250)
            {
                LoadLevel(next);
            }
            else
            {
                Debug.Log("[GameplayController] Congratulations! All 250 levels completed!");
                LoadLevel(1); // loop back
            }
        }

        // Hint Actions
        public void UseRevealLetterHint()
        {
            if (EconomyManager.Instance.TryPurchaseHint(HintType.RevealLetter))
            {
                _usedHintThisLevel = true;
                if (_gridModel.RevealRandomLetter(out var cell))
                {
                    if (crosswordRenderer != null) crosswordRenderer.AnimateHintCell(cell);
                    if (_gridModel.IsComplete) StartCoroutine(CompleteLevelRoutine());
                }
            }
        }

        public void UseSmartHint()
        {
            if (EconomyManager.Instance.TryPurchaseHint(HintType.SmartHint))
            {
                _usedHintThisLevel = true;
                if (_gridModel.RevealSmartLetter(out var cell))
                {
                    if (crosswordRenderer != null) crosswordRenderer.AnimateHintCell(cell);
                    if (_gridModel.IsComplete) StartCoroutine(CompleteLevelRoutine());
                }
            }
        }

        public void UseRevealWordHint()
        {
            if (EconomyManager.Instance.TryPurchaseHint(HintType.RevealWord))
            {
                _usedHintThisLevel = true;
                if (_gridModel.RevealEntireWord(out var placement))
                {
                    if (crosswordRenderer != null) crosswordRenderer.AnimateWordSolved(placement);
                    if (_gridModel.IsComplete) StartCoroutine(CompleteLevelRoutine());
                }
            }
        }

        public void OnShuffleClicked()
        {
            if (letterWheel != null)
            {
                letterWheel.ShuffleStones();
            }
        }

        private void UpdateCoinDisplay(int newBalance, int delta)
        {
            if (coinCountText != null)
            {
                coinCountText.text = newBalance.ToString();
            }
        }

        private void LoadBundledDictionary()
        {
            // Starter dictionary of high-frequency English words
            string[] starterWords = new[]
            {
                "THE", "AND", "YOU", "THAT", "WAS", "FOR", "ARE", "WITH", "HIS", "THEY",
                "ONE", "HAVE", "THIS", "FROM", "WORD", "WHAT", "SOME", "TIME", "LOOK",
                "MORE", "WRITE", "GO", "SEE", "NUMBER", "NO", "WAY", "COULD", "PEOPLE",
                "MY", "THAN", "FIRST", "WATER", "BEEN", "CALL", "WHO", "OIL", "ITS",
                "NOW", "FIND", "LONG", "DOWN", "DAY", "DID", "GET", "COME", "MADE", "MAY",
                "PART", "NEW", "SOUND", "TAKE", "ONLY", "LITTLE", "WORK", "KNOW", "PLACE",
                "YEAR", "LIVE", "ME", "BACK", "GIVE", "MOST", "VERY", "AFTER", "THING",
                "OUR", "JUST", "NAME", "GOOD", "SENTENCE", "MAN", "THINK", "SAY", "GREAT",
                "WHERE", "HELP", "THROUGH", "MUCH", "BEFORE", "LINE", "RIGHT", "TOO", "MEAN",
                "OLD", "ANY", "SAME", "TELL", "BOY", "FOLLOW", "CAME", "WANT", "SHOW", "ALSO",
                "AROUND", "FARM", "THREE", "SMALL", "SET", "PUT", "END", "DOES", "ANOTHER",
                "WELL", "LARGE", "MUST", "BIG", "EVEN", "SUCH", "BECAUSE", "TURN", "HERE",
                "WHY", "ASK", "WENT", "MEN", "READ", "NEED", "LAND", "DIFFERENT", "HOME", "US",
                "MOVE", "TRY", "KIND", "HAND", "PICTURE", "AGAIN", "CHANGE", "OFF", "PLAY",
                "SPELL", "AIR", "AWAY", "HOUSE", "POINT", "PAGE", "LETTER", "MOTHER", "ANSWER",
                "FOUND", "STUDY", "STILL", "LEARN", "SHOULD", "AMERICA", "WORLD", "HIGH",
                "EVERY", "NEAR", "ADD", "FOOD", "BETWEEN", "OWN", "BELOW", "COUNTRY", "PLANT",
                "LAST", "SCHOOL", "FATHER", "KEEP", "TREE", "NEVER", "START", "CITY", "EARTH",
                "EYES", "LIGHT", "THOUGHT", "HEAD", "UNDER", "STORY", "SAW", "FAR", "SEA",
                "DRAW", "LEFT", "LATE", "RUN", "DON", "WHILE", "PRESS", "CLOSE", "NIGHT",
                "REAL", "LIFE", "FEW", "STOP", "OPEN", "SEEM", "TOGETHER", "NEXT", "WHITE",
                "CHILDREN", "BEGIN", "GOT", "WALK", "EXAMPLE", "EASE", "PAPER", "OFTEN",
                "ALWAYS", "MUSIC", "THOSE", "BOTH", "MARK", "BOOK", "LETTER", "UNTIL", "MILE",
                "RIVER", "CAR", "FEET", "CARE", "SECOND", "GROUP", "CARRY", "TOOK", "RAIN",
                "EAT", "ROOM", "FRIEND", "BEGAN", "FISH", "MOUNTAIN", "NORTH", "ONCE", "BASE",
                "HEAR", "HORSE", "CUT", "SURE", "WATCH", "COLOR", "FACE", "WOOD", "MAIN",
                "STONE", "LAKE", "ROSE", "VALLEY", "LEAF", "GREEN", "FLOWER", "GOLD", "CRYSTAL"
            };

            foreach (var w in starterWords)
            {
                _dictionary.Insert(w);
            }
        }
    }
}
