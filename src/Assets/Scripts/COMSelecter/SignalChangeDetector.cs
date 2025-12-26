using UnityEngine;

public class SignalChangeDetector
{
    int previousValue_;
    int currentValue_;
    bool isFirst_;
    bool updateFlag_;


    public SignalChangeDetector(int x)
    {
        isFirst_ = true;
        previousValue_ = x;
    }

    public void Update()
    {
        if (updateFlag_)
        {
            previousValue_ = currentValue_;
            isFirst_ = false;
            updateFlag_ = false;
        }
    }

    public void Input(int x)
    {
        updateFlag_ = true;
        currentValue_ = x;
    }

    public bool IsChanged()
    {
        if (isFirst_ || currentValue_ != previousValue_)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}