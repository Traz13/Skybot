using UnityEngine;
using System.Collections;

//	Building type the type of building will be the related to the shape and general use
//	- Model shapes for Uniformity across building Styles
public enum BuildingUse
{
	Apartments = 0, // 3x7
	Tower,
	Count
};

//	Building sytles are visual alternates of a Building Type
//	- Textures and Shaders
public enum BuildingTheme
{
	Slums	= 0,		//	Run down buildings
	General,			//	General population
	Corporation,		//	Copoorate HQ
	Citadel,			// 	Utopian
	HackSpace,			//	Digitized version of reality
	Count
};

public enum BlockType
{
	TopEdge = 0,
	TopMiddle,
	Edge,
	Middle,
	AltEdge,
	AltMiddle,
	BottomEdge,
	BottomMiddle,
	Count
};

public enum ProjectileInfo
{
	ShotVariance = 30,
};