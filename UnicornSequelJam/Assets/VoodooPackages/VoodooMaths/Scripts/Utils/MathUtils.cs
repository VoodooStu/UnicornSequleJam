using System;
using System.Collections.Generic;
using UnityEngine;

namespace VoodooPackages.Tech
{
	public static class Arithmetic
	{
		/// <summary>
		/// Returnes _Value at the power _Power.
		/// </summary>
		/// <returns>The power.</returns>
		/// <param name="_Value">Value.</param>
		/// <param name="_Power">Power.</param>
		public static int IntPower(int _Value, int _Power)
		{
			return (int)Math.Pow(_Value, _Power);
		}

		/// <summary>
		/// Remap _Value from [_MinIn;_MaxIn] to [_MinOut;_MaxOut].
		/// </summary>
		/// <returns>The remaped value.</returns>
		/// <param name="_Value">Value.</param>
		/// <param name="_MinIn">Minimum in.</param>
		/// <param name="_MaxIn">Max in.</param>
		/// <param name="_MinOut">Minimum out.</param>
		/// <param name="_MaxOut">Max out.</param>
		public static float Remap(float _Value, float _MinIn, float _MaxIn, float _MinOut, float _MaxOut)
		{
			return _MinOut + (_Value - _MinIn) * (_MaxOut - _MinOut) / (_MaxIn - _MinIn);
		}

		/// <summary>
		/// Remap _Value from [_MinIn;_MaxIn] to [_MinOut;_MaxOut] with a step of _Step
		/// </summary>
		/// <returns>The remaped value.</returns>
		/// <param name="_Value">Value.</param>
		/// <param name="_MinIn">Minimum in.</param>
		/// <param name="_MaxIn">Max in.</param>
		/// <param name="_MinOut">Minimum out.</param>
		/// <param name="_MaxOut">Max out.</param>
		/// <param name="_Step">Step.</param>
		public static float Remap(float _Value, float _MinIn, float _MaxIn, float _MinOut, float _MaxOut, float _Step)
		{
			return _MinOut + Mathf.FloorToInt((_Value - _MinIn) * (_MaxOut - _MinOut) / ((_MaxIn - _MinIn)* _Step))* _Step;
		}
	}

	public static class Geometry
	{
		/// <summary>
		/// Gets the polygon surface giving its point positions.
		/// </summary>
		/// <returns>The polygon surface.</returns>
		/// <param name="_Points">Points</param>
		public static float GetPolygonSurface(List<Vector2> _Points)
		{
			float temp = 0;
			int i = 0;

			for (; i < _Points.Count; i++)
			{
				if (i != _Points.Count - 1)
				{
					float mulA = _Points[i].x * _Points[i + 1].y;
					float mulB = _Points[i + 1].x * _Points[i].y;
					temp += mulA - mulB;
				}
				else
				{
					float mulA = _Points[i].x * _Points[0].y;
					float mulB = _Points[0].x * _Points[i].y;
					temp += mulA - mulB;
				}
			}

			temp *= 0.5f;
			return Mathf.Abs(temp);
		}

		/// <summary>
		/// Is the given point _Position near to one of the positions given in _Points.
		/// </summary>
		/// <returns><c>true</c> if position is close to one of the points in _Points, <c>false</c> otherwise.</returns>
		/// <param name="_Points">Points</param>
		/// <param name="_Position">Point Position.</param>
		/// <param name="_SqrThreshold">Square threshold.</param>
		public static bool IsPositionCloseToPath(List<Vector2> _Points, Vector2 _Position, float _SqrThreshold)
		{
			for (int i = 0; i < _Points.Count; ++i)
			{
				if ((_Points[i] - _Position).sqrMagnitude < _SqrThreshold)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Removes all the point positions _Points from _RefPath if they are close enough.
		/// </summary>
		/// <param name="_RefPath">Reference path. Most of the time correspond to point positions</param>
		/// <param name="_Points">Points</param>
		/// <param name="_SqrThreshold">Square threshold.</param>
		public static void RemoveClosePathToAnother(List<Vector2> _RefPath, List<Vector2> _Points, float _SqrThreshold)
		{
			for (int i = 0; i < _Points.Count; ++i)
			{
				if (IsPositionCloseToPath(_RefPath, _Points[i], _SqrThreshold))
				{
					_Points.RemoveAt(i);
					i--;
				}
			}
		}

		/// <summary>
		/// Removes the close points in _Points.
		/// </summary>
		/// <param name="_Points">Points</param>
		/// <param name="_SqrThreshold">Sqr threshold.</param>
		/// <param name="_IsPathLooped">If set to <c>true</c> loop from the last element to the first one.</param>
		public static void RemoveClosePointsInPath(List<Vector2> _Points, float _SqrThreshold, bool _IsPathLooped)
		{
			for (int i = 0; i < _Points.Count - 1; ++i)
			{
				if ((_Points[i] - _Points[i + 1]).sqrMagnitude < _SqrThreshold)
					_Points.RemoveAt(i + 1);
			}

			if (_IsPathLooped && (_Points[_Points.Count - 1] - _Points[0]).sqrMagnitude < _SqrThreshold)
				_Points.RemoveAt(0);
		}

		/// <summary>
		/// Gets the length of the path.
		/// </summary>
		/// <returns>The path length.</returns>
		/// <param name="_Points">Points</param>
		/// <param name="_IsPathLooped">If set to <c>true</c> loop from the last element to the first one.</param>
		public static float GetPathLength(List<Vector2> _Points, bool _IsPathLooped)
		{
			float sqrLength = 0.0f;

			for (int i = 0; i < _Points.Count - 1; ++i)
				sqrLength += (_Points[i + 1] - _Points[i]).sqrMagnitude;

			if (_IsPathLooped)
				sqrLength += (_Points[0] - _Points[_Points.Count - 1]).sqrMagnitude;

			return Mathf.Sqrt(sqrLength);
		}

		/// <summary>
		/// Points time on the line.
		/// </summary>
		/// <returns>The time on the line.</returns>
		/// <param name="_P1">P1.</param>
		/// <param name="_P2">P2.</param>
		/// <param name="_P">P.</param>
		public static float PointTimeOnLine(Vector2 _P1, Vector2 _P2, Vector2 _P)
		{
			Vector2 pointDiff = _P - _P1;
			Vector2 lineDiff = _P2 - _P1;
			float lineNorm = lineDiff.magnitude;
			return Vector2.Dot(pointDiff, lineDiff) / (lineNorm * lineNorm);
		}

		/// <summary>
		/// Points time on the segment.
		/// </summary>
		/// <returns>The time on segment.</returns>
		/// <param name="_P1">P1.</param>
		/// <param name="_P2">P2.</param>
		/// <param name="_P">P.</param>
		public static float PointTimeOnSegment(Vector2 _P1, Vector2 _P2, Vector2 _P)
		{
			return Mathf.Clamp01(PointTimeOnLine(_P1, _P2, _P));
		}

		/// <summary>
		/// Points on the segment.
		/// </summary>
		/// <returns>Point on the segment.</returns>
		/// <param name="_P1">P1.</param>
		/// <param name="_P2">P2.</param>
		/// <param name="_P">P.</param>
		public static Vector2 PointOnSegment(Vector2 _P1, Vector2 _P2, Vector2 _P)
		{
			return PointOnLine(_P1, _P2, PointTimeOnSegment(_P1, _P2, _P));
		}

		/// <summary>
		/// Points on the line.
		/// </summary>
		/// <returns>Point on the line.</returns>
		/// <param name="_P1">P1.</param>
		/// <param name="_P2">P2.</param>
		/// <param name="_P">P.</param>
		public static Vector2 PointOnLine(Vector2 _P1, Vector2 _P2, Vector2 _P)
		{
			return PointOnLine(_P1, _P2, PointTimeOnLine(_P1, _P2, _P));
		}

		/// <summary>
		/// Points the on line.
		/// </summary>
		/// <returns>The on line.</returns>
		/// <param name="_P1">P1.</param>
		/// <param name="_P2">P2.</param>
		/// <param name="_Time">Time.</param>
		public static Vector2 PointOnLine(Vector2 _P1, Vector2 _P2, float _Time)
		{
			return _P1 + _Time * (_P2 - _P1);
		}

		/// <summary>
		/// Nearests the point on segment.
		/// </summary>
		/// <returns>The point on segment.</returns>
		/// <param name="_P1">P1.</param>
		/// <param name="_P2">P2.</param>
		/// <param name="_P">P.</param>
		/// <param name="_IsFirst">If set to <c>true</c> is first.</param>
		/// <param name="_IsSecond">If set to <c>true</c> is second.</param>
		/// <param name="_Projection">Projection.</param>
		public static float NearestPointOnSegment(Vector2 _P1, Vector2 _P2, Vector2 _P, out bool _IsFirst, out bool _IsSecond, ref Vector2 _Projection)
		{
			float time = PointTimeOnSegment(_P1, _P2, _P);

			// The nearest entry point is the first point of the segment
			if (time < 0.05f)
			{
				_IsFirst = true;
				_IsSecond = false;
				return (_P - _P1).magnitude;
			}
			// The nearest entry point is the second point of the segment
			else if (time > 0.95f)
			{
				_IsFirst = false;
				_IsSecond = true;
				return (_P - _P2).magnitude;
			}
			// The nearest point is between first and second point of the segment, so we need to compute the projection
			else
			{
				_IsFirst = false;
				_IsSecond = false;
				_Projection = PointOnLine(_P1, _P2, time);
				return (_P - _Projection).magnitude;
			}
		}

		/// <summary>
		/// The point distance from the line
		/// </summary>
		/// <returns>The point distance from the line.</returns>
		/// <param name="_P1">P1.</param>
		/// <param name="_P2">P2.</param>
		/// <param name="_P">P.</param>
		public static float DistancePointToLine(Vector2 _P1, Vector2 _P2, Vector2 _P)
		{
			return (PointOnLine(_P1, _P2, _P) - _P).magnitude;
		}

		/// <summary>
		/// Is shape convex.
		/// </summary>
		/// <returns><c>true</c> if the shape formed by the _Points is convex, <c>false</c> otherwise.</returns>
		/// <param name="_Points">Points.</param>
		public static bool IsConvexShape(Vector2[] _Points)
		{
			if (_Points.Length < 4)
				return true;

			bool sign = false;
			for (int i = 0; i < _Points.Length; i++)
			{
				float dx1 = _Points[(i + 2) % _Points.Length].x - _Points[(i + 1) % _Points.Length].x;
				float dy1 = _Points[(i + 2) % _Points.Length].y - _Points[(i + 1) % _Points.Length].y;
				float dx2 = _Points[i].x - _Points[(i + 1) % _Points.Length].x;
				float dy2 = _Points[i].y - _Points[(i + 1) % _Points.Length].y;
				float zcrossproduct = dx1 * dy2 - dy1 * dx2;

				if (i == 0)
					sign = zcrossproduct > 0;
				else if (sign != (zcrossproduct > 0))
					return false;
			}

			return true;
		}

		/// <summary>
		/// Makes the shape simple.
		/// </summary>
		/// <param name="_Points">Points.</param>
		public static void MakeShapeSimple(List<Vector2> _Points)
		{
			// First segment loop
			for (int i = 0; i < _Points.Count; i += 2)
			{
				// Second segment loop
				for (int j = 0; j < _Points.Count; j += 2)
				{
					// Skip the same segment comparison
					if (i == j)
						continue;

					int nextI = i + 1;
					if (i == _Points.Count - 1)
						nextI = 0;

					int nextJ = j + 1;
					if (j == _Points.Count - 1)
						nextJ = 0;

					if (SegmentIntersects(_Points[i], _Points[nextI], _Points[j], _Points[nextJ]))
					{
						_Points.RemoveRange(i, j - i);
						i -= j - i;
						j -= j - i;
					}
				}
			}
		}


		/// <summary>
		/// Does [PO,P1] intersects [P2,P3] ?.
		/// </summary>
		/// <returns><c>true</c> if the segment formed by P0,P1 intersects the segment formed by P2,P3, <c>false</c> otherwise.</returns>
		/// <param name="_P0"></param>
		/// <param name="_P1"></param>
		/// <param name="_P2"></param>
		/// <param name="_P3"></param>
		/// <param name="_IncludeSimilarPoints"></param>
		public static bool SegmentIntersects(Vector2 _P0, Vector2 _P1, Vector2 _P2, Vector2 _P3, bool _IncludeSimilarPoints = false)
		{
			float s1_x, s1_y, s2_x, s2_y;
			s1_x = _P1.x - _P0.x;
			s1_y = _P1.y - _P0.y;
			s2_x = _P3.x - _P2.x;
			s2_y = _P3.y - _P2.y;

			float s, t;
			s = (-s1_y * (_P0.x - _P2.x) + s1_x * (_P0.y - _P2.y)) / (-s2_x * s1_y + s1_x * s2_y);
			t = (s2_x * (_P0.y - _P2.y) - s2_y * (_P0.x - _P2.x)) / (-s2_x * s1_y + s1_x * s2_y);

			bool res = s >= Mathf.Epsilon && s <= 1f - Mathf.Epsilon && t >= Mathf.Epsilon && t <= 1f - Mathf.Epsilon;

			if (res)
				return true;

			if (_IncludeSimilarPoints)
				res = s >= 0 && s <= 1f && t >= 0 && t <= 1f;

			return res;
		}

		/// <summary>
		/// Does [PO,P1] intersects [P2,P3] ?.
		/// </summary>
		/// <returns><c>true</c> if the segment formed by P0,P1 intersects the segment formed by P2,P3, <c>false</c> otherwise.</returns>
		/// <param name="_P0">P0.</param>
		/// <param name="_P1">P1.</param>
		/// <param name="_P2">P2.</param>
		/// <param name="_P3">P3.</param>
		/// <param name="_Intersection">Intersection.</param>
		public static bool SegmentIntersects(Vector2 _P0, Vector2 _P1, Vector2 _P2, Vector2 _P3, ref Vector2 _Intersection)
		{
			float s1_x, s1_y, s2_x, s2_y;
			s1_x = _P1.x - _P0.x;
			s1_y = _P1.y - _P0.y;
			s2_x = _P3.x - _P2.x;
			s2_y = _P3.y - _P2.y;

			float s, t;
			s = (-s1_y * (_P0.x - _P2.x) + s1_x * (_P0.y - _P2.y)) / (-s2_x * s1_y + s1_x * s2_y);
			t = (s2_x * (_P0.y - _P2.y) - s2_y * (_P0.x - _P2.x)) / (-s2_x * s1_y + s1_x * s2_y);

			if (s >= 0.0f && s <= 1.0f && t >= 0.0f && t <= 1.0f)
			{
				_Intersection.x = _P0.x + (t * s1_x);
				_Intersection.y = _P0.y + (t * s1_y);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Is the point inside triangle.
		/// </summary>
		/// <returns><c>true</c>, if point _Point is inside the triangle formed by _P0, _P1, _P2, <c>false</c> otherwise.</returns>
		/// <param name="_P0">P0.</param>
		/// <param name="_P1">P1.</param>
		/// <param name="_P2">P2.</param>
		/// <param name="_Point">Point.</param>
		public static bool IsPointInsideTriangle(Vector2 _P0, Vector2 _P1, Vector2 _P2, Vector2 _Point)
		{
			float A = 0.5f * (-_P1.y * _P2.x + _P0.y * (-_P1.x + _P2.x) + _P0.x * (_P1.y - _P2.y) + _P1.x * _P2.y);
			float sign = A < 0f ? -1f : 1f;
			float s = (_P0.y * _P2.x - _P0.x * _P2.y + (_P2.y - _P0.y) * _Point.x + (_P0.x - _P2.x) * _Point.y) * sign;
			float t = (_P0.x * _P1.y - _P0.y * _P1.x + (_P0.y - _P1.y) * _Point.x + (_P1.x - _P0.x) * _Point.y) * sign;
			return s > 0f && t > 0f && (s + t) < 2f * A * sign;
		}

		/// <summary>
		/// Finds the normal of the plane formed by _LastPosition, _Position, _Reference.
		/// </summary>
		/// <returns>The normal.</returns>
		/// <param name="_LastPosition">Last position.</param>
		/// <param name="_Position">Position.</param>
		/// <param name="_Reference">Reference.</param>
		public static Vector3 FindNormal(Vector3 _LastPosition, Vector3 _Position, Vector3 _Reference)
		{
			Vector3 lastToPos = _Position - _LastPosition;
			Vector3 lastToRef = _Reference - _LastPosition;
			float diffAngle = Vector3.SignedAngle(lastToPos, lastToRef, Vector3.forward);
			return Vector3.Cross(lastToPos, Mathf.Sign(diffAngle) * Vector3.forward).normalized;
		}

		/// <summary>
		/// Is the point in polygon.
		/// </summary>
		/// <returns><c>true</c> if the point is in the polygon, <c>false</c> otherwise.</returns>
		/// <param name="_P">P.</param>
		/// <param name="_Polygon">Polygon.</param>
		public static bool IsPointInPolygon(Vector2 _P, Vector2[] _Polygon)
		{
			int i, j;
			Vector2 minMaxX = new Vector2(_Polygon[0].x, _Polygon[0].x);
			Vector2 minMaxY = new Vector2(_Polygon[0].y, _Polygon[0].y);

			for (i = 1; i < _Polygon.Length; i++)
			{
				Vector2 p = _Polygon[i];
				minMaxX.x = Math.Min(p.x, minMaxX.x);
				minMaxX.y = Math.Max(p.x, minMaxX.y);
				minMaxY.x = Math.Min(p.y, minMaxY.x);
				minMaxY.y = Math.Max(p.y, minMaxY.y);
			}

			if (_P.x < minMaxX.x || _P.x > minMaxX.y || _P.y < minMaxY.x || _P.y > minMaxY.y)
				return false;

			bool inside = false;
			for (i = 0, j = _Polygon.Length - 1; i < _Polygon.Length; j = i++)
			{
				if ((_Polygon[i].y > _P.y) != (_Polygon[j].y > _P.y) &&
					 _P.x < (_Polygon[j].x - _Polygon[i].x) * (_P.y - _Polygon[i].y) / (_Polygon[j].y - _Polygon[i].y) + _Polygon[i].x)
				{
					inside = !inside;
				}
			}

			return inside;
		}

		/// <summary>
		/// Rotates the point _Point around the point _Pivot with an angle of _Angle
		/// </summary>
		/// <param name="_Point"></param>
		/// <param name="_Pivot"></param>
		/// <param name="_Angle"></param>
		/// <returns> The rotated point</returns>
		public static Vector2 RotatePoint(Vector2 _Point, Vector2 _Pivot, float _Angle)
		{
			float rad = _Angle * Mathf.Deg2Rad;
			float x = Mathf.Cos(rad) * (_Point.x - _Pivot.x) - Mathf.Sin(rad) * (_Point.y - _Pivot.y) + _Pivot.x;
			float y = Mathf.Sin(rad) * (_Point.x - _Pivot.x) + Mathf.Cos(rad) * (_Point.y - _Pivot.y) + _Pivot.y;

			return new Vector2(x, y);
		}
	}
}