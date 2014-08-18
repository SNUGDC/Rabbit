using UnityEngine;
using System.Collections;

public class Diamond{
	public Vector2 mCenter;
	public float mHWidth;
	public float mHHeight;

	public Diamond(){
		mCenter = new Vector2(0, 0);
		mHWidth = 0;
		mHHeight = 0;
	}

	public Diamond(Vector2 center, float halfWidth, float halfHeight){
		mCenter = new Vector2(center.x, center.y);
		mHWidth = halfWidth;
		mHHeight = halfHeight;
	}

	public bool Contains(Vector2 input){
		if(input.x == mCenter.x){
			return (mCenter.y - mHHeight <= input.y) && (input.y <= mCenter.y + mHHeight);
		}
		else if(mCenter.y - mHHeight <= input.y && input.y <= mCenter.y){
			float resultLeft = Mathf.Abs(((mCenter.y - mHHeight) - input.y) * mHWidth);
			float resultRight = Mathf.Abs((mCenter.x - input.x) * mHHeight);
			return resultLeft < -resultRight || resultLeft > resultRight;
		}
		else if(mCenter.y <= input.y && input.y <= mCenter.y + mHHeight){
			float resultLeft = Mathf.Abs(((mCenter.y + mHHeight) - input.y) * mHWidth);
			float resultRight = Mathf.Abs((mCenter.x - input.x) * mHHeight);
			return resultLeft < -resultRight || resultLeft > resultRight;
		}
		else{
			return false;
		}
	}

	public Vector2 top(){
		return new Vector2(mCenter.x, mCenter.y + mHHeight);
	}

	public Vector2 bottom(){
		return new Vector2(mCenter.x, mCenter.y - mHHeight);
	}

	public Vector2 left(){
		return new Vector2(mCenter.x - mHWidth, mCenter.y);
	}

	public Vector2 right(){
		return new Vector2(mCenter.x + mHWidth, mCenter.y);
	}
}
