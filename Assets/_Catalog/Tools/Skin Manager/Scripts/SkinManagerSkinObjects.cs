using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SkinManagerSkinObjects
{
	public SkinObject Sphere = new SkinObject(SkinnableType.Sphere);
	public SkinObject Cube = new SkinObject(SkinnableType.Cube);
	public SkinObject Plane = new SkinObject(SkinnableType.Plane);
	public SkinObject Cylinder = new SkinObject(SkinnableType.Cylinder);
	public SkinObject Blabla = new SkinObject(SkinnableType.Blabla);
	public SkinObject Particles = new SkinObject(SkinnableType.Particles);
	public SkinObject Dimitry = new SkinObject(SkinnableType.Dimitry);
	public SkinObject Light = new SkinObject(SkinnableType.Light);


	public Dictionary<SkinnableType, SkinObject> GetLookup()
	{
		Dictionary<SkinnableType, SkinObject> skinLookup = new Dictionary<SkinnableType, SkinObject>()
		{
			{ SkinnableType.Sphere, Sphere },
			{ SkinnableType.Cube, Cube },
			{ SkinnableType.Plane, Plane },
			{ SkinnableType.Cylinder, Cylinder },
			{ SkinnableType.Blabla, Blabla },
			{ SkinnableType.Particles, Particles },
			{ SkinnableType.Dimitry, Dimitry },
			{ SkinnableType.Light, Light },
		};

		return skinLookup;
	}
}
