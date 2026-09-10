using System.Collections.Generic;

namespace WordVista.Generator
{
    public static class LevelCatalog
    {
        public static List<PuzzleTemplate> GetPuzzles()
        {
            var list = new List<PuzzleTemplate>();

            void Add(string root, string[] answers, string[] bonuses = null)
            {
                list.Add(new PuzzleTemplate
                {
                    RootLetters = root.ToUpperInvariant(),
                    Answers = new List<string>(answers),
                    Bonuses = bonuses != null ? new List<string>(bonuses) : new List<string>()
                });
            }

            // ==========================================
            // WORLD 1: GREEN VALLEY (Levels 1 - 30)
            // Levels 1-20: 3-4 letters
            // Levels 21-30: 4-5 letters
            // ==========================================
            Add("ACT", new[] { "CAT", "ACT" }, new[] { "AT" });
            Add("TOP", new[] { "POT", "TOP" }, new[] { "OPT" });
            Add("NOW", new[] { "WON", "NOW" }, new[] { "OWN", "NO" });
            Add("SUN", new[] { "SUN", "US" }, new[] { "NUR" });
            Add("DOG", new[] { "GOD", "DOG" }, new[] { "DO", "GO" });
            Add("PET", new[] { "PET", "NET" }, new[] { "PEN", "TEN" });
            Add("CAR", new[] { "ARC", "CAR" }, new[] { "OAR" });
            Add("BED", new[] { "BED", "RED" }, new[] { "BE" });
            Add("CUP", new[] { "CUP", "CAP" }, new[] { "UP" });
            Add("HAT", new[] { "HAT", "THE" }, new[] { "AT", "HE" });
            Add("PIG", new[] { "PIG", "PIN" }, new[] { "PI", "NIP" });
            Add("BUS", new[] { "BUS", "SUB" }, new[] { "US", "TUB" });
            Add("FOX", new[] { "FOX", "BOX" }, new[] { "OF", "OX" });
            Add("KEY", new[] { "KEY", "EYE" }, new[] { "YE" });
            Add("MAP", new[] { "MAP", "PAN" }, new[] { "AM", "MAN" });
            Add("BAT", new[] { "BAT", "TAB" }, new[] { "AT", "BET" });
            Add("OWL", new[] { "OWL", "LOW" }, new[] { "BOW" });
            Add("LOG", new[] { "LOG", "FOG" }, new[] { "LEG", "GO" });
            Add("JAM", new[] { "JAM", "DAM" }, new[] { "AM", "RAM" });
            Add("FAN", new[] { "FAN", "VAN" }, new[] { "AN", "RAN" });

            // Levels 21 - 30 (4-5 letters)
            Add("STAR", new[] { "STAR", "RATS", "ART" }, new[] { "TAR", "RAT" });
            Add("LEAF", new[] { "LEAF", "ALE", "FLEA" }, new[] { "ELF" });
            Add("ROSE", new[] { "ROSE", "SORE", "ORES" }, new[] { "ROE", "ORE" });
            Add("TREE", new[] { "TREE", "FREE" }, new[] { "TEE", "RET" });
            Add("BIRD", new[] { "BIRD", "DIRT" }, new[] { "BID", "RIB" });
            Add("WIND", new[] { "WIND", "WILD" }, new[] { "WIN", "DIN" });
            Add("RIVER", new[] { "RIVER", "RIVE", "ERR" }, new[] { "REV", "IRE" });
            Add("STONE", new[] { "STONE", "TONES", "NOTE" }, new[] { "ONE", "NET", "SET" });
            Add("GRASS", new[] { "GRASS", "RAGS", "SAG" }, new[] { "GAS" });
            Add("BLOOM", new[] { "BLOOM", "BOOM", "LOOM" }, new[] { "MOO" });

            // ==========================================
            // WORLD 2: MYSTIC FOREST (Levels 31 - 60)
            // 4-5 letters
            // ==========================================
            Add("WOOD", new[] { "WOOD", "GOOD" }, new[] { "WOO" });
            Add("MOSS", new[] { "MOSS", "SOME" }, new[] { "SO" });
            Add("ROOT", new[] { "ROOT", "TOOR" }, new[] { "TOO", "ROT" });
            Add("BARK", new[] { "BARK", "DARK" }, new[] { "BAR", "ARK" });
            Add("MIST", new[] { "MIST", "SMIT" }, new[] { "ITS", "SIT" });
            Add("TWIG", new[] { "TWIG", "WING" }, new[] { "GIT", "WIT" });
            Add("FERN", new[] { "FERN", "FREE" }, new[] { "REF", "FEN" });
            Add("DEER", new[] { "DEER", "REED" }, new[] { "RED", "DEE" });
            Add("BEAR", new[] { "BEAR", "BARE" }, new[] { "EAR", "BAR" });
            Add("WOLF", new[] { "WOLF", "FLOW" }, new[] { "LOW", "OWL" });
            Add("PINE", new[] { "PINE", "SPIN" }, new[] { "PIN", "PIE" });
            Add("SEED", new[] { "SEED", "DEED" }, new[] { "SEE", "DEE" });
            Add("PATH", new[] { "PATH", "THAT" }, new[] { "HAT", "PAT" });
            Add("CAMP", new[] { "CAMP", "RAMP" }, new[] { "CAP", "MAP" });
            Add("GLOW", new[] { "GLOW", "BLOW" }, new[] { "LOW", "LOG" });
            Add("MAZE", new[] { "MAZE", "GAZE" }, new[] { "JAM" });
            Add("SHADE", new[] { "SHADE", "HEAD", "SHED" }, new[] { "ASH", "HAD", "HAS" });
            Add("GROVE", new[] { "GROVE", "OVER", "ROVE" }, new[] { "ORE", "EGO" });
            Add("FAIRY", new[] { "FAIRY", "AIRY", "FAIR" }, new[] { "FAR", "RAY", "AIR" });
            Add("MAGIC", new[] { "MAGIC", "GAME" }, new[] { "AIM", "MAG" });
            Add("ACORN", new[] { "ACORN", "ROAN", "CORN" }, new[] { "OAR", "CAN", "NOR" });
            Add("IVORY", new[] { "IVORY", "ROVE" }, new[] { "RAY", "IVY" });
            Add("CREEK", new[] { "CREEK", "REEK" }, new[] { "EKE" });
            Add("FLORA", new[] { "FLORA", "ROAR" }, new[] { "FOR", "OAR", "FAR" });
            Add("TRAIL", new[] { "TRAIL", "RAIL", "TAIL" }, new[] { "LIT", "AIR", "ART" });
            Add("OASIS", new[] { "OASIS", "SOAP" }, new[] { "ISO", "SOS" });
            Add("LODGE", new[] { "LODGE", "GOLD" }, new[] { "LOG", "DOG", "LED" });
            Add("CABIN", new[] { "CABIN", "COIN" }, new[] { "BIN", "CAN", "BAN" });
            Add("EARTH", new[] { "EARTH", "HEART", "HARE" }, new[] { "HAT", "THE", "EAR" });
            Add("NATURE", new[] { "NATURE", "TUNA", "TRUE" }, new[] { "RUN", "NET", "RUT" });

            // ==========================================
            // WORLD 3: CRYSTAL LAKE (Levels 61 - 90)
            // Levels 61-70: 5 letters
            // Levels 71-90: 5-6 letters
            // ==========================================
            Add("CLEAR", new[] { "CLEAR", "RACE", "ACRE" }, new[] { "ALE", "CAR", "EAR" });
            Add("WATER", new[] { "WATER", "TEAR", "RATE" }, new[] { "WET", "AWE", "WAR" });
            Add("SHINE", new[] { "SHINE", "SHIN" }, new[] { "SHE", "HEN", "HIS" });
            Add("SHORE", new[] { "SHORE", "HORSE" }, new[] { "SHE", "ORE", "ROE" });
            Add("WAVES", new[] { "WAVES", "WAVE", "SAVE" }, new[] { "SAW", "SEW" });
            Add("FLOAT", new[] { "FLOAT", "LOAF", "FLAT" }, new[] { "FAT", "OAT", "LOT" });
            Add("DRIFT", new[] { "DRIFT", "DIRT", "RIFT" }, new[] { "FIT", "RID" });
            Add("TIDES", new[] { "TIDES", "TIDE", "DIET" }, new[] { "TIE", "DIE" });
            Add("OCEAN", new[] { "OCEAN", "CANOE" }, new[] { "ONE", "CAN" });
            Add("PEARL", new[] { "PEARL", "PALE", "LEAP" }, new[] { "APE", "EAR", "PEA" });
            Add("STREAM", new[] { "STREAM", "TEAM", "MEAT" }, new[] { "MAT", "SEA", "RAT" });
            Add("SPRING", new[] { "SPRING", "RINGS", "PING" }, new[] { "PIG", "PIN", "SIP" });
            Add("ISLAND", new[] { "ISLAND", "LANDS", "SAIL" }, new[] { "LAD", "AID", "AND" });
            Add("CORAL", new[] { "CORAL", "COAL", "COLA" }, new[] { "OAR", "CAR" });
            Add("SWIMS", new[] { "SWIMS", "SWIM" }, new[] { "IS" });
            Add("RIPPLE", new[] { "RIPPLE", "PIPER", "PIPE" }, new[] { "LIP", "RIP", "PER" });
            Add("BEACH", new[] { "BEACH", "EACH", "ACHE" }, new[] { "BEE", "ACE" });
            Add("SHELL", new[] { "SHELL", "HELL", "SELL" }, new[] { "SHE", "ELL" });
            Add("ANCHOR", new[] { "ANCHOR", "ROACH", "ARCH" }, new[] { "OAR", "CAN", "NOR" });
            Add("SAILOR", new[] { "SAILOR", "SOLAR", "SAIL" }, new[] { "AIR", "OIL", "SIR" });
            Add("HARBOR", new[] { "HARBOR", "ROAR", "BOAR" }, new[] { "BAR", "OAR" });
            Add("DIVING", new[] { "DIVING", "DING" }, new[] { "DIG", "GIN" });
            Add("LAGOON", new[] { "LAGOON", "GOAL", "LOAN" }, new[] { "LOG", "NOG" });
            Add("MARINA", new[] { "MARINA", "RAIN", "MAIN" }, new[] { "AIM", "ARM", "MAN" });
            Add("GEYSER", new[] { "GEYSER", "GREY", "EYES" }, new[] { "YES", "SEE" });
            Add("SPLASH", new[] { "SPLASH", "SLASH", "PASS" }, new[] { "ASH", "LAP", "PAL" });
            Add("SURFS", new[] { "SURFS", "SURF", "FURS" }, new[] { "FUR" });
            Add("CANAL", new[] { "CANAL", "CLAN" }, new[] { "CAN" });
            Add("PONDS", new[] { "PONDS", "POND" }, new[] { "POD", "NOD" });
            Add("CRYSTAL", new[] { "CRYSTAL", "STRAY", "CLAY" }, new[] { "CRY", "SAY", "ACT", "CAT" });

            // ==========================================
            // WORLD 4: DESERT KINGDOM (Levels 91 - 120)
            // 5-6 letters
            // ==========================================
            Add("DUNES", new[] { "DUNES", "DUES", "SEND" }, new[] { "SUE", "END", "DUE" });
            Add("SANDS", new[] { "SANDS", "SAND" }, new[] { "AND", "SAD" });
            Add("PALMS", new[] { "PALMS", "PALM", "SLAP" }, new[] { "MAP", "LAP", "PAL" });
            Add("CAMEL", new[] { "CAMEL", "MALE", "CLAM" }, new[] { "ALE", "ACE", "MAC" });
            Add("SOLAR", new[] { "SOLAR", "SOAR", "ORAL" }, new[] { "OAR", "SOL" });
            Add("GOLDEN", new[] { "GOLDEN", "LODGE", "LONG" }, new[] { "GOLD", "OLD", "DOG", "ONE" });
            Add("CANYON", new[] { "CANYON", "CANON", "COIN" }, new[] { "CAN", "ANY" });
            Add("TEMPLE", new[] { "TEMPLE", "MELT", "PELT" }, new[] { "MET", "LET", "PET" });
            Add("MIRAGE", new[] { "MIRAGE", "GAME", "GEAR" }, new[] { "AIM", "ARM", "RAG" });
            Add("PYRAMID", new[] { "PYRAMID", "PRAY", "DAMP" }, new[] { "MAY", "PAY", "RAY", "AIM" });
            Add("SPHINX", new[] { "SPHINX", "SPIN", "SHIN" }, new[] { "NIP", "PIN", "SIX" });
            Add("CROWNS", new[] { "CROWNS", "CROWN", "CROW" }, new[] { "ROW", "COW", "OWN" });
            Add("THRONE", new[] { "THRONE", "NORTH", "HORN" }, new[] { "ONE", "NET", "HER", "HOT" });
            Add("RELICS", new[] { "RELICS", "RELIC", "RICE" }, new[] { "ICE", "LIE" });
            Add("SCEPTER", new[] { "SCEPTER", "CREPT", "PEST" }, new[] { "PET", "SET", "TEE" });
            Add("VAULTS", new[] { "VAULTS", "VAULT", "LAST" }, new[] { "VAT" });
            Add("TREASUR", new[] { "TREASUR", "TRUE", "REST" }, new[] { "EAT", "SEE", "TAR" });
            Add("JEWELS", new[] { "JEWELS", "JEWEL" }, new[] { "JEW" });
            Add("COBRAS", new[] { "COBRAS", "COBRA", "CRAB" }, new[] { "BAR", "OAR", "CAR" });
            Add("SCARAB", new[] { "SCARAB", "BRASS", "SCAR" }, new[] { "BAR", "ARC" });
            Add("HEATS", new[] { "HEATS", "HEAT", "HATE" }, new[] { "HAT", "THE", "EAT" });
            Add("WARMS", new[] { "WARMS", "WARM" }, new[] { "ARM", "WAR", "RAW" });
            Add("SUNSET", new[] { "SUNSET", "TUNES", "NEST" }, new[] { "SUN", "SET", "NUT" });
            Add("SHADOW", new[] { "SHADOW", "SHOW", "WASH" }, new[] { "ASH", "HAD", "SOW" });
            Add("VALLEY", new[] { "VALLEY", "LAVE" }, new[] { "ALE", "LAY" });
            Add("BRONZE", new[] { "BRONZE", "ZERO", "ZONE" }, new[] { "ONE", "ROB" });
            Add("SILVER", new[] { "SILVER", "LIVER", "VEIL" }, new[] { "SIR", "LIE" });
            Add("PALACE", new[] { "PALACE", "PLACE", "LEAP" }, new[] { "ACE", "ALE", "CAP" });
            Add("EMPIRE", new[] { "EMPIRE", "PRIME", "RIPE" }, new[] { "PIE", "RIM", "PER" });
            Add("SULTAN", new[] { "SULTAN", "LAST", "SALT" }, new[] { "SUN", "NUT", "ANT" });

            // ==========================================
            // WORLD 5: SNOW VALLEY (Levels 121 - 150)
            // 5-6 letters
            // ==========================================
            Add("FROST", new[] { "FROST", "FORT", "SOFT" }, new[] { "FOR", "ROT" });
            Add("CHILL", new[] { "CHILL", "HILL" }, new[] { "ILL" });
            Add("GLACIER", new[] { "GLACIER", "CLEAR", "RACE" }, new[] { "ALE", "ICE", "EAR" });
            Add("SPEAKS", new[] { "SPEAKS", "SPEAK", "PEAK" }, new[] { "APE", "ASK", "SEA" });
            Add("ALPINE", new[] { "ALPINE", "PLANE", "LEAP" }, new[] { "PIN", "PIE", "ALE" });
            Add("SUMMIT", new[] { "SUMMIT", "MINT", "SUIT" }, new[] { "SIT", "NUT" });
            Add("SLOPES", new[] { "SLOPES", "SLOPE", "POLES" }, new[] { "LIP" });
            Add("RIDGES", new[] { "RIDGES", "RIDGE", "DIRE" }, new[] { "RED", "DIG" });
            Add("CABINS", new[] { "CABINS", "CABIN" }, new[] { "BIN", "CAN" });
            Add("FIRES", new[] { "FIRES", "FIRE", "RIFE" }, new[] { "REF", "FEE" });
            Add("TIMBER", new[] { "TIMBER", "TRIBE", "TERM" }, new[] { "BET", "RIM", "BIT" });
            Add("LIZARDS", new[] { "LIZARDS", "LIZARD", "BIRD" }, new[] { "BAD", "LAD", "BAR" });
            Add("WINTER", new[] { "WINTER", "TWIN", "TIRE" }, new[] { "WIN", "NET", "TIE" });
            Add("FLURRY", new[] { "FLURRY", "FURL", "FURY" }, new[] { "FLY", "FUR" });
            Add("ICICLE", new[] { "ICICLE", "LICE" }, new[] { "ICE", "LIE" });
            Add("FREEZE", new[] { "FREEZE", "FEVER", "REED" }, new[] { "FEE", "REF" });
            Add("POLAR", new[] { "POLAR", "SOLAR", "ROAR" }, new[] { "OAR" });
            Add("ARCTIC", new[] { "ARCTIC", "CART", "TACT" }, new[] { "CAT", "ACT", "ART" });
            Add("TUNDRA", new[] { "TUNDRA", "DAUNT", "TURN" }, new[] { "NUT", "RUT", "RUN" });
            Add("SLEDS", new[] { "SLEDS", "SLED", "LEST" }, new[] { "LED" });
            Add("SKATE", new[] { "SKATE", "STAKE", "TAKE" }, new[] { "ATE", "TEA", "SET" });
            Add("GLOVES", new[] { "GLOVES", "GLOVE", "LOVE" }, new[] { "LOG", "LEG" });
            Add("SCARFS", new[] { "SCARFS", "SCARF" }, new[] { "FAR", "CAR" });
            Add("BOOTS", new[] { "BOOTS", "BOOT", "ROOT" }, new[] { "TOO", "BOO" });
            Add("COCOAS", new[] { "COCOAS", "COOK", "COAL" }, new[] { "COO" });
            Add("HEARTH", new[] { "HEARTH", "EARTH", "HEART" }, new[] { "HAT", "THE", "EAR" });
            Add("SPRUCE", new[] { "SPRUCE", "SUPER", "PURE" }, new[] { "CUP", "USE", "PER" });
            Add("CEDAR", new[] { "CEDAR", "CARE", "RACE" }, new[] { "RED", "EAR", "ARC" });
            Add("VALLEYS", new[] { "VALLEYS", "VALLEY" }, new[] { "ALE", "LAY" });
            Add("CHILLY", new[] { "CHILLY", "CHILL", "HILL" }, new[] { "ILL" });

            // ==========================================
            // WORLD 6: TROPICAL ISLAND (Levels 151 - 180)
            // 6 letters
            // ==========================================
            Add("JUNGLE", new[] { "JUNGLE", "LUNGE", "GLUE" }, new[] { "JUG", "LUG", "GUN" });
            Add("ISLAND", new[] { "ISLAND", "LANDS" }, new[] { "LAD", "AND", "SIN" });
            Add("MONKEY", new[] { "MONKEY", "MONK", "OMEN" }, new[] { "ONE", "KEY", "MEN" });
            Add("PARROT", new[] { "PARROT", "ROAR", "PART" }, new[] { "POT", "TOP", "RAP" });
            Add("TOUCAN", new[] { "TOUCAN", "COUNT", "COAT" }, new[] { "CAT", "OUT", "NUT" });
            Add("BANANA", new[] { "BANANA" }, new[] { "BAN" });
            Add("PAPAYA", new[] { "PAPAYA", "PRAY" }, new[] { "PAY" });
            Add("COCONUT", new[] { "COCONUT", "COUNT", "ONTO" }, new[] { "NUT", "OUT", "COT" });
            Add("ORCHID", new[] { "ORCHID", "CHORD", "RICH" }, new[] { "ROD", "RID", "HID" });
            Add("BAMBOO", new[] { "BAMBOO", "BOMB", "BOOM" }, new[] { "BOO", "MOB" });
            Add("CANOPY", new[] { "CANOPY", "PONY", "COPY" }, new[] { "PAN", "CAP", "ANY" });
            Add("VINES", new[] { "VINES", "VINE", "VEIN" }, new[] { "VIE", "SIN" });
            Add("LIZARD", new[] { "LIZARD", "RAID", "DIAL" }, new[] { "LAD", "AID", "RID" });
            Add("IGUANA", new[] { "IGUANA", "AGAIN", "GAIN" }, new[] { "GUN" });
            Add("GECKOS", new[] { "GECKOS", "GECKO" }, new[] { "EGO" });
            Add("TURTLE", new[] { "TURTLE", "RULE", "LUTE" }, new[] { "LET", "RUT" });
            Add("DOLPHIN", new[] { "DOLPHIN", "HOLD", "LION" }, new[] { "LIP", "PIN", "HIP" });
            Add("SHARKS", new[] { "SHARKS", "SHARK", "RASH" }, new[] { "ASH", "ARK" });
            Add("REEFS", new[] { "REEFS", "REEF", "FREE" }, new[] { "FEE", "REF" });
            Add("PADDLE", new[] { "PADDLE", "PLEAD", "PALE" }, new[] { "PAD", "LAD", "APE" });
            Add("KAYAKS", new[] { "KAYAKS", "KAYAK" }, new[] { "YAK" });
            Add("HARBOR", new[] { "HARBOR", "ROAR" }, new[] { "BAR", "OAR" });
            Add("SAILS", new[] { "SAILS", "SAIL" }, new[] { "AIL" });
            Add("BREEZE", new[] { "BREEZE", "BEER", "ZERO" }, new[] { "BEE" });
            Add("TROPIC", new[] { "TROPIC", "TOPIC", "PORT" }, new[] { "TIP", "TOP", "POT" });
            Add("SUNNY", new[] { "SUNNY", "SUNS" }, new[] { "SUN" });
            Add("WARMTH", new[] { "WARMTH", "TRAM" }, new[] { "WAR", "MAT", "HAT" });
            Add("FLAMING", new[] { "FLAMING", "FOAM", "LOAF" }, new[] { "AIM", "FIG", "FOG" });
            Add("VOLCANO", new[] { "VOLCANO", "VOCAL", "COAL" }, new[] { "CAN", "VAN" });
            Add("PARADIS", new[] { "PARADIS", "PRIDE", "SPEAR" }, new[] { "SEA", "RED", "EAR", "PIE" });

            // ==========================================
            // WORLD 7: SKY KINGDOM (Levels 181 - 215)
            // 6 letters
            // ==========================================
            Add("CLOUDS", new[] { "CLOUDS", "CLOUD", "COLD" }, new[] { "OLD" });
            Add("CASTLE", new[] { "CASTLE", "SCALE", "LATE" }, new[] { "CAT", "ACT", "LET", "SET" });
            Add("TOWERS", new[] { "TOWERS", "TOWER", "WROTE" }, new[] { "TWO", "WET", "TOE" });
            Add("BRIDGE", new[] { "BRIDGE", "RIDGE", "BIRD" }, new[] { "BED", "RED", "BIG" });
            Add("PALACE", new[] { "PALACE", "PLACE", "LACE" }, new[] { "CAP", "ALE", "APE" });
            Add("FLIGHT", new[] { "FLIGHT", "LIGHT", "GIFT" }, new[] { "FIT", "LIT", "HIT" });
            Add("WINGS", new[] { "WINGS", "SWING", "WINS" }, new[] { "WIN", "SIN" });
            Add("EAGLES", new[] { "EAGLES", "EAGLE", "GALE" }, new[] { "ALE", "AGE" });
            Add("FALCON", new[] { "FALCON", "COAL", "FLAN" }, new[] { "FAN", "CAN" });
            Add("FEATHER", new[] { "FEATHER", "HEART", "EARTH" }, new[] { "HAT", "THE", "EAR", "EAT" });
            Add("SOARS", new[] { "SOARS", "SOAR", "ROAR" }, new[] { "OAR" });
            Add("GLIDES", new[] { "GLIDES", "GLIDE", "IDLE" }, new[] { "LED", "DIG" });
            Add("HORIZON", new[] { "HORIZON", "HORN", "IRON" }, new[] { "ZOO", "NOR" });
            Add("SUNRISE", new[] { "SUNRISE", "RISEN", "SIRE" }, new[] { "SUN", "SIN", "RUN" });
            Add("TWILIGH", new[] { "TWILIGH", "LIGHT", "WITH" }, new[] { "LIT", "HIT", "WIG" });
            Add("CELESTI", new[] { "CELESTI", "STEAL", "LEAST" }, new[] { "LET", "SET", "TEA" });
            Add("ETHERS", new[] { "ETHERS", "ETHER", "THERE" }, new[] { "THE", "HER" });
            Add("SPIRIT", new[] { "SPIRIT", "STRIP", "TRIP" }, new[] { "SIT", "TIP", "SIR" });
            Add("TEMPLES", new[] { "TEMPLES", "TEMPLE", "PELT" }, new[] { "PET", "MET", "LET" });
            Add("SANCTUR", new[] { "SANCTUR", "SCANT", "STRAY" }, new[] { "SAY", "ACT", "CAT" });
            Add("ZEPHYR", new[] { "ZEPHYR", "PREY", "HYPE" }, new[] { "RYE", "PER" });
            Add("AURUMS", new[] { "AURUMS", "AURUM", "RAMS" }, new[] { "ARM", "RUM" });
            Add("SILVERS", new[] { "SILVERS", "SILVER", "LIVE" }, new[] { "LIE", "SIR" });
            Add("MYSTIC", new[] { "MYSTIC", "MISTY", "CITY" }, new[] { "ITS", "SIT" });
            Add("PORTAL", new[] { "PORTAL", "PATROL", "PORT" }, new[] { "POT", "TOP", "OAR" });
            Add("REALMS", new[] { "REALMS", "REALM", "MEAL" }, new[] { "ALE", "EAR", "ARM" });
            Add("DOMINIO", new[] { "DOMINIO", "MIND", "MOON" }, new[] { "DIM", "MID" });
            Add("EMPIRES", new[] { "EMPIRES", "EMPIRE", "PRIME" }, new[] { "PIE", "PER" });
            Add("LEGEND", new[] { "LEGEND", "GLEN", "NEED" }, new[] { "LED", "END" });
            Add("MYTHICS", new[] { "MYTHICS", "MYTHIC", "MINT" }, new[] { "HIT" });
            Add("VALORS", new[] { "VALORS", "VALOR", "ORAL" }, new[] { "OAR" });
            Add("HEROIC", new[] { "HEROIC", "ECHO", "HIRE" }, new[] { "ICE", "HER" });
            Add("SHIELD", new[] { "SHIELD", "SLIDE", "HIDE" }, new[] { "SHE", "LED" });
            Add("SWORDS", new[] { "SWORDS", "SWORD", "WORDS" }, new[] { "ROD", "ROW" });
            Add("INFINIT", new[] { "INFINIT", "NIFTY", "TINY" }, new[] { "FIT", "TIN" });

            // ==========================================
            // WORLD 8: AURORA WORLD (Levels 216 - 250)
            // Levels 216-220: 6 letters
            // Levels 221-250: 6-7 letters
            // ==========================================
            Add("AURORA", new[] { "AURORA", "ROAR", "AURA" }, new[] { "OAR" });
            Add("COSMOS", new[] { "COSMOS", "MOSS" }, new[] { "SOO" });
            Add("GALAXY", new[] { "GALAXY", "GALA" }, new[] { "LAY" });
            Add("NEBULA", new[] { "NEBULA", "BANE", "BALE" }, new[] { "BAN", "ALE" });
            Add("STELLAR", new[] { "STELLAR", "STARE", "TEARS" }, new[] { "LET", "SET", "RAT" });
            Add("ECLIPSE", new[] { "ECLIPSE", "PIECE", "SLICE" }, new[] { "ICE", "LIP", "PIE" });
            Add("METEOR", new[] { "METEOR", "REMOTE", "METER" }, new[] { "MET", "ROT", "TOE" });
            Add("COMETS", new[] { "COMETS", "COMET", "SMOTE" }, new[] { "MET", "COT" });
            Add("PLANET", new[] { "PLANET", "PLATE", "PLANT" }, new[] { "LET", "NET", "PAN" });
            Add("ORBITS", new[] { "ORBITS", "ORBIT" }, new[] { "BIT", "ROT" });
            Add("GRAVITY", new[] { "GRAVITY", "VIGOR", "GRAY" }, new[] { "RAG", "RAY", "ART" });
            Add("SPECTRUM", new[] { "SPECTR", "SUPER", "CREPT" }, new[] { "CUP", "PET", "USE" });
            Add("PRISMS", new[] { "PRISMS", "PRISM" }, new[] { "RIM", "SIR" });
            Add("RADIANC", new[] { "RADIANC", "DRAIN" }, new[] { "RED", "AID", "AND" });
            Add("LUMENS", new[] { "LUMENS", "LUMEN", "MULES" }, new[] { "MEN", "SUN" });
            Add("BEACON", new[] { "BEACON", "BACON", "ONCE" }, new[] { "ONE", "CAN", "BOA" });
            Add("LANTERN", new[] { "LANTERN", "LEARN", "ALERT" }, new[] { "LET", "NET", "ART" });
            Add("CANDLE", new[] { "CANDLE", "LANCE", "CLEAN" }, new[] { "CAN", "AND", "LED" });
            Add("SPARKLE", new[] { "SPARKLE", "SPEAK", "SPARK" }, new[] { "APE", "ARK", "ALE" });
            Add("TWINKLE", new[] { "TWINKLE", "KNELT", "TWIN" }, new[] { "LET", "WIN", "TIN" });
            Add("GLIMMER", new[] { "GLIMMER", "MIME", "LIME" }, new[] { "LEG", "GEM", "RIM" });
            Add("SHIMMER", new[] { "SHIMMER", "HEIR" }, new[] { "SHE", "HER", "HIM" });
            Add("DIAMOND", new[] { "DIAMOND", "DOMAIN", "MAID" }, new[] { "DIM", "AND", "MAD" });
            Add("EMERALD", new[] { "EMERALD", "DREAM", "REALM" }, new[] { "RED", "ARM", "EAR" });
            Add("SAPPHIR", new[] { "SAPPHIR", "SHIRE", "SHARE" }, new[] { "SHE", "EAR", "HIP" });
            Add("AMETHYS", new[] { "AMETHYS", "STEAM", "MEAT" }, new[] { "MAT", "SAT", "YES" });
            Add("QUARTZ", new[] { "QUARTZ", "QUAT" }, new[] { "RUT", "TAR" });
            Add("OBSIDIA", new[] { "OBSIDIA", "BASIN" }, new[] { "BAD", "BIN", "SAD" });
            Add("CELESTS", new[] { "CELESTS", "CASTLE", "STALE" }, new[] { "LET", "SET", "CAT" });
            Add("HARMONY", new[] { "HARMONY", "MANOR", "ROAM" }, new[] { "ARM", "RAY", "MAY" });
            Add("SERENIT", new[] { "SERENIT", "ENTRY", "TREES" }, new[] { "SEE", "YES", "NET" });
            Add("ETERNIT", new[] { "ETERNIT", "TIRE", "TEEN" }, new[] { "TIE", "NET", "YET" });
            Add("HORIZON", new[] { "HORIZON", "SHORN", "IRONS" }, new[] { "SON", "NOR", "HIS" });
            Add("ODYSSEY", new[] { "ODYSSEY", "DOSE" }, new[] { "YES", "DYE" });
            Add("WORDVIS", new[] { "WORDVIS", "SAVIOR", "TRAIL" }, new[] { "AIR", "OAR", "STAR", "VOW" });

            return list;
        }
    }
}
