using Godot;
public static class BezierTween
{
	public static Tween PositionQuadratic(
	Control target,
	Vector2 startPoint,
	Vector2 endPoint,
	float angleDeg,
	double duration,
	bool global = false,
	Tween.TransitionType transition = Tween.TransitionType.Linear,
	Tween.EaseType ease = Tween.EaseType.InOut)
	{
		float tanA = Mathf.Tan(Mathf.DegToRad(angleDeg));
		float tanB = Mathf.Tan(Mathf.DegToRad(90f - angleDeg));

		Vector2 midPoint;

		if (Mathf.IsZeroApprox(Mathf.Cos(Mathf.DegToRad(angleDeg))))
		{
			midPoint = new Vector2(startPoint.X, endPoint.Y);
		}
		else if (Mathf.IsZeroApprox(Mathf.Cos(Mathf.DegToRad(90f - angleDeg))))
		{
			midPoint = new Vector2(endPoint.X, startPoint.Y);
		}
		else
		{
			Vector2 delta = endPoint - startPoint;
			float xRel = (tanB * delta.X + delta.Y) / (tanA + tanB);
			float yRel = tanA * xRel;
			midPoint = startPoint + new Vector2(xRel, yRel);
		}

		return global
			? PositionQuadraticGlobal(target, startPoint, midPoint, endPoint, duration, transition, ease)
			: PositionQuadratic(target, startPoint, midPoint, endPoint, duration, transition, ease);
	}
	public static Tween PositionQuadratic(
		Control target,
		Vector2 p0,
		Vector2 p1,
		Vector2 p2,
		double duration,
		Tween.TransitionType transition = Tween.TransitionType.Linear,
		Tween.EaseType ease = Tween.EaseType.InOut)
	{
		Tween tween = target.CreateTween();
		tween.SetTrans(transition).SetEase(ease);

		tween.TweenMethod(
			Callable.From<float>(t =>
			{
				target.Position = Quadratic(p0, p1, p2, t);
			}),
			0.0f,
			1.0f,
			duration
		);

		return tween;
	}
	public static Tween PositionQuadraticGlobal(
		Control target,
		Vector2 p0,
		Vector2 p1,
		Vector2 p2,
		double duration,
		Tween.TransitionType transition = Tween.TransitionType.Linear,
		Tween.EaseType ease = Tween.EaseType.InOut)
	{
		Tween tween = target.CreateTween();
		tween.SetTrans(transition).SetEase(ease);

		tween.TweenMethod(
			Callable.From<float>(t =>
			{
				target.GlobalPosition = Quadratic(p0, p1, p2, t);
			}),
			0.0f,
			1.0f,
			duration
		);

		return tween;
	}

	public static Tween PositionCubic(
		Node2D target,
		Vector2 p0,
		Vector2 p1,
		Vector2 p2,
		Vector2 p3,
		double duration,
		Tween.TransitionType transition = Tween.TransitionType.Linear,
		Tween.EaseType ease = Tween.EaseType.InOut)
	{
		Tween tween = target.CreateTween();
		tween.SetTrans(transition).SetEase(ease);

		tween.TweenMethod(
			Callable.From<float>(t =>
			{
				target.Position = Cubic(p0, p1, p2, p3, t);
			}),
			0.0f,
			1.0f,
			duration
		);

		return tween;
	}

	public static Vector2 Quadratic(Vector2 p0, Vector2 p1, Vector2 p2, float t)
	{
		Vector2 q0 = p0.Lerp(p1, t);
		Vector2 q1 = p1.Lerp(p2, t);
		return q0.Lerp(q1, t);
	}

	public static Vector2 Cubic(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
	{
		Vector2 q0 = p0.Lerp(p1, t);
		Vector2 q1 = p1.Lerp(p2, t);
		Vector2 q2 = p2.Lerp(p3, t);

		Vector2 r0 = q0.Lerp(q1, t);
		Vector2 r1 = q1.Lerp(q2, t);

		return r0.Lerp(r1, t);
	}
}
