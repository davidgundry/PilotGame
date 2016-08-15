﻿using UnityEngine;
using System.Collections;
using System;
using MicTools;

namespace MicTools
{
public delegate void SyllableDetectedHandler();

[RequireComponent(typeof(MicrophoneController))]
[AddComponentMenu("MicrophoneTools/MicrophoneInput")]
public class MicrophoneInput : MonoBehaviour
{
    public float Pitch {get; set;}
    public int syllableCount;
    public event SyllableDetectedHandler OnSyllablePeak;

    private MicrophoneController microphoneController;
    private Yin yin;
    private SyllableDetectionAlgorithm syllableDetectionAlgorithm;

    /// <summary>
    /// Current window-average intensity level
    /// </summary>
    public float Level { get { if (syllableDetectionAlgorithm != null) return syllableDetectionAlgorithm.Level; else return -1; } }
    /// <summary>
    /// Current autocorrelation
    /// </summary>
    public float NormalisedPeakAutocorrelation { get { if (syllableDetectionAlgorithm != null) return syllableDetectionAlgorithm.NormalisedPeakAutocorrelation; else return -1; } }

    void Awake()
    {
        microphoneController = GetComponent<MicrophoneController>();
    }

    public void OnSoundEvent(SoundEvent e)
    {
        switch (e)
        {
            case SoundEvent.BufferReady:
                yin = new Yin(microphoneController.SampleRate, SyllableDetectionAlgorithm.windowSize);
                LogMT.Log("Window Size for Algorithm: " + SyllableDetectionAlgorithm.windowSize);

                syllableDetectionAlgorithm = new SyllableDetectionAlgorithm(microphoneController.SampleRate);
                break;
        }
    }

    void Update()
    {
        float[] window = microphoneController.GetMostRecentSamples(SyllableDetectionAlgorithm.windowSize);
            
        if (yin != null)
            Pitch = yin.getPitch(window);

        if (syllableDetectionAlgorithm != null)
            if (syllableDetectionAlgorithm.Run(window, Time.deltaTime))
            {
                //gameObject.SendMessage("OnSoundEvent", SoundEvent.SyllablePeak, SendMessageOptions.DontRequireReceiver);
                OnSyllablePeak.Invoke();
                syllableCount++;
            }
    }

    /// <summary>
    /// Run the algorithm over an AudioClip instead of real-time input.
    /// </summary>
    /// <param name="testClip">The AudioClip containing the test data</param>
    /// <returns>Number of syllables counted</returns>
    public static int RunTest(AudioClip testClip)
    {
        SyllableDetectionAlgorithm testSDA = new SyllableDetectionAlgorithm(testClip.frequency);

        int syllables = 0;
        if (testClip != null)
        {
            float[] samples = new float[SyllableDetectionAlgorithm.windowSize];

            for (int offset = 0; offset < testClip.samples; offset += SyllableDetectionAlgorithm.windowSize)
            {
                if (offset + SyllableDetectionAlgorithm.windowSize > testClip.samples)
                    samples = new float[testClip.samples - offset];

                testClip.GetData(samples, offset);
                if (testSDA.Run(samples, ((float) samples.Length) / ((float) testClip.frequency )))
                    syllables++;
            }
           

            /*for (int i = 0; i < testClip.samples; i += length)
            {
                if (i + length > testClip.samples)
                    samples = new float[testClip.samples - i];

                testClip.GetData(samples, i);
                if (testSDA.Run(samples, samples.Length / testClip.frequency))
                    syllables++;
            }*/
    }
        else
            throw new ArgumentNullException("Cannot test Microphone Input without a test clip.");

        return syllables;
    }

}
}
