namespace MotivationalLizard;

public struct TimeEvent
{
	public float ActivationTime { get; set; }
	public int CameraToUse { get; set; }
	public int LabelToActivate { get; set; }
	public bool FadeIn { get; set; }
	public bool FadeOut { get; set; }

	public TimeEvent(float activationTime, int cameraToUse, int labelToActivate, bool fadeIn, bool fadeOut)
	{
		ActivationTime = activationTime;
		CameraToUse = cameraToUse;
		LabelToActivate = labelToActivate;
		FadeIn = fadeIn;
		FadeOut = fadeOut;
	}
}
