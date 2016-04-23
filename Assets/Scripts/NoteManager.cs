using UnityEngine;
using System.Collections.Generic;
using GAudio;
using System;


[RequireComponent(typeof(PulseModule))]
public class NoteManager : MonoBehaviour, IGATPulseClient
{
	public delegate void NotePass(List<PlayInstructs> instructs, ConstFile.Notes note);
	public ConstFile.Notes note;
	public event NotePass PlayNotesEvent;
	public GATActiveSampleBank sampleBank;
	public List<PlayInstructs> NoteQue;
	PulseModule pulse;
	PulseModule _Pulse
	{
		get
		{
			if (pulse == null)
			{
				pulse = GetComponent<PulseModule>();
			}
			return pulse;
		}
	}
	

	void Start()
	{
		NoteQue = new List<PlayInstructs>();
	}


	public void OnPulse(IGATPulseInfo pulseInfo)
	{
		if (PlayNotesEvent != null)
		{
			PlayNotesEvent(NoteQue, note);
		}
		NoteQue.Clear();
	}
	
	public void PulseStepsDidChange(bool[] newSteps)
	{
		throw new NotImplementedException();
	}

	public void AddInstruct(PlayInstructs newInstruct)
	{
		NoteQue.Add(newInstruct);
	}

	void OnEnable()
	{
		_Pulse.SubscribeToPulse(this);
	}

	void OnDisable()
	{
		_Pulse.UnsubscribeToPulse(this);
	}
}
