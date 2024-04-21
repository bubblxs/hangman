using System.Text;

public class Word
{
    private string _word;

    public Word(string word)
    {
        _word = word;
    }

    public string GetWord()
    {
        return _word;
    }

    public List<int> GetIndexesOf(char letter)
    {
        List<int> idx = new List<int>();

        for (int i = 0, l = _word.Length; i < l; i++)
        {
            if (string.Equals(_word[i].ToString(), letter.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                idx.Add(i);
            }
        }

        return idx;
    }

    public bool IsEqual(char[] word)
    {
        return IsEqual(new string(word));
    }

    public bool IsEqual(string word)
    {
        return string.Equals(_word, word, StringComparison.OrdinalIgnoreCase);
    }

    public string BuildWordMask()
    {
        const char maskChar = '_';
        string mask = new(string.Empty);

        for (int i = 0, l = _word.Length; i < l; i++)
        {
            mask += _word[i] == ' ' ? ' ' : maskChar;
        }

        return mask;
    }
}