using CardCalculator.Models;

namespace CardCalculator.Calculator
{
	public class CardCalculator
  {
    public CardCalculator()
    {
      ErrorMessage = Exceptions.Exceptions.NoException;
    }

    public CardCalculator(string hand)
    {
      FinalScore = 0;
      ErrorMessage = Exceptions.Exceptions.NoException;

      #region Normalize Data

      if (hand.Contains(" "))
        hand = hand.Replace(" ", "");

      if (hand != "")
      {
        hand = hand.ToUpper();

        // Build playing card deck
        string[] numberValues = "2,3,4,5,6,7,8,9,t,j,q,k,a".ToUpper().Split(",");
        string[] suits = "c,d,h,s".ToUpper().Split(",");
        List<string> validCards = new List<string>();

        foreach (var suit in suits)
          foreach (var value in numberValues)
            validCards.Add($"{value}{suit}");

        validCards.Add("JK");
        validCards.Add("JK");

        // check for invalid characters
        string invalidCharactersString = "<,>,@,.,?,/,:,;,',~,#,{,[,],\\,},!,\",£,$,%,^,&,*,(,),_,+,-,=";
        List<string> invalidCharacters = invalidCharactersString.Split(",").ToList();

        foreach (char character in hand)
          foreach (string invalid in invalidCharacters)
            if (character == Convert.ToChar(invalid))
            {
              ErrorMessage = Exceptions.Exceptions.InvalidEntry;
              return;
            }

        // Split into string array
        _hand = hand.Split(",").ToList();

        // Check all cards are valid
        foreach (string card in _hand)
          if (!validCards.Contains(card))
          {
            ErrorMessage = Exceptions.Exceptions.UnrecognisedValue;
            return;
          }

        // Check for duplicates
        List<string> duplicates = _hand.GroupBy(x => x)
                .Where(g => g.Count() > 1)
                .Select(y => y.Key)
                .ToList();

        foreach (string card in duplicates)
          if (card != "JK")
          {
            ErrorMessage = Exceptions.Exceptions.DuplicateCard;
            return;
          }

        #endregion

        #region Sort Data

        // iterating over all elements of array
        foreach (string card in _hand)
        {
          // if card is joker, insert into joker array
          if ((card == "JK") || (card == "jk"))
            _jokers.Add(new Card()
            {
              Value = "JK",
              Suit = null,
              isJoker = true
            });
          // if card has value, insert into value array
          else if ((card != "JK") || (card != "jk"))
          {
            int valueTest = CheckValue(cardString: card, cardObject: null);
            if (valueTest == -1)
            {
              ErrorMessage = Exceptions.Exceptions.UnrecognisedValue;
              return;
            }

            int multiplierTest = CheckMultiplier(cardString: card, cardObject: null);
            if (multiplierTest == -1)
            {
              ErrorMessage = Exceptions.Exceptions.UnrecognisedValue;
              return;
            }

            _valueCards.Add(new Card()
            {
              Value = Convert.ToString(card[0]),
              Suit = Convert.ToString(card[1]),
              isJoker = false
            });

          }
        }

        #endregion

        CalculateFinalScore(_valueCards, _jokers);
      }
    }

    #region Calculations

    public int CheckValue(string? cardString, Card? cardObject)
    {
      int value = 0;

      if (cardString != null)
      {
        switch (cardString[0])
        {
          case '2':
            value = 2;
            break;
          case '3':
            value = 3;
            break;
          case '4':
            value = 4;
            break;
          case '5':
            value = 5;
            break;
          case '6':
            value = 6;
            break;
          case '7':
            value = 7;
            break;
          case '8':
            value = 8;
            break;
          case '9':
            value = 9;
            break;
          case 't':
          case 'T':
            value = 10;
            break;
          case 'j':
          case 'J':
            value = 11;
            break;
          case 'q':
          case 'Q':
            value = 12;
            break;
          case 'k':
          case 'K':
            value = 13;
            break;
          case 'a':
          case 'A':
            value = 14;
            break;
          default:
            ErrorMessage = Exceptions.Exceptions.UnrecognisedValue;
            value = -1;
            break;
        }
      }
      else if (cardObject != null)
      {
        switch (cardObject.Value)
        {
          case "2":
            value = 2;
            break;
          case "3":
            value = 3;
            break;
          case "4":
            value = 4;
            break;
          case "5":
            value = 5;
            break;
          case "6":
            value = 6;
            break;
          case "7":
            value = 7;
            break;
          case "8":
            value = 8;
            break;
          case "9":
            value = 9;
            break;
          case "t":
          case "T":
            value = 10;
            break;
          case "j":
          case "J":
            value = 11;
            break;
          case "q":
          case "Q":
            value = 12;
            break;
          case "k":
          case "K":
            value = 13;
            break;
          case "a":
          case "A":
            value = 14;
            break;
          default:
            ErrorMessage = Exceptions.Exceptions.UnrecognisedValue;
            value = -1;
            break;
        }
      }

      return value;
    }

    // Method will only be called for one variable type
    public int CheckMultiplier(string? cardString, Card? cardObject)
    {
      int multiplier = 0;

      if (cardString != null)
      {
        switch (cardString[1])
        {
          case 'C':
          case 'c':
            multiplier = 1;
            break;
          case 'D':
          case 'd':
            multiplier = 2;
            break;
          case 'H':
          case 'h':
            multiplier = 3;
            break;
          case 'S':
          case 's':
            multiplier = 4;
            break;
          default:
            ErrorMessage = Exceptions.Exceptions.UnrecognisedValue;
            multiplier = -1;
            break;
        }
      }
      else if (cardObject != null)
      {
        switch (cardObject.Suit)
        {
          case "C":
          case "c":
            multiplier = 1;
            break;
          case "D":
          case "d":
            multiplier = 2;
            break;
          case "H":
          case "h":
            multiplier = 3;
            break;
          case "S":
          case "s":
            multiplier = 4;
            break;
          default:
            ErrorMessage = Exceptions.Exceptions.UnrecognisedValue;
            multiplier = -1;
            break;
        }
      }

      return multiplier;
    }

    public int CalculateFinalScore(List<Card>? values, List<Card>? jokers)
    {
      bool calculated = false;
      foreach (Card card in values)
        FinalScore += (CheckValue(cardString: null, cardObject: card) * CheckMultiplier(cardString: null, cardObject: card));

      while (!calculated)
      {
        if (_jokers.Count <= 2)
        {
          foreach (Card joker in jokers)
            FinalScore *= 2;
          calculated = true;
        }
        else if (_jokers.Count >= 3)
        {
          ErrorMessage = Exceptions.Exceptions.TooManyJokers;


          while (_jokers.Count > 2)
          {
            _jokers.Remove(_jokers[_jokers.Count - 1]);
          }

          continue;
        }
      }
      return FinalScore;
    }

    #endregion

    #region Getters
    public int GetFinalScore()
    {
      return FinalScore;
    }

    public List<List<Card>> GetCardList()
    {
      return new List<List<Card>>()
      {
        _valueCards,
        _jokers
      };
    }

    #endregion

    #region Public Members

    public static int FinalScore { get; set; } = 0;
    public static string ErrorMessage { get; set; }
    public static List<string> _hand { get; set; }
    public static int _totalScore { get; set; }

    public List<Card> _jokers = new List<Card>();
    public List<Card> _valueCards = new List<Card>();

    #endregion
  }
}
