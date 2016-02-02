using UnityEngine;
using System.Collections;

public class VoteCharacter : MonoBehaviour {

	private Transform[] _transform;
	private Renderer[] _renderer;
	private Texture[] _texture, _overTexture, _deadTexture;
	private bool[] _isTextureLocked;
	private int _selectedIndex;
	
	void Start(){	
		// Initialise voting prefab here
		int _numOfOptions = 5;
		_texture = new Texture[_numOfOptions];
		_texture[0] = Resources.Load("Textures/Drum/not select") as Texture;
		_texture[1] = Resources.Load("Textures/Character/Heads/Character 1/Brown Hair Unselected") as Texture;
		_texture[2] = Resources.Load("Textures/Character/Heads/Character 2/Black Hair Unselected") as Texture;
		_texture[3] = Resources.Load("Textures/Character/Heads/Character 3/Blonde Hair Unselected") as Texture;
		_texture[4] = Resources.Load("Textures/Character/Heads/Character 4/Red Hair Unselected") as Texture;
		
		_overTexture = new Texture[_numOfOptions];
		_overTexture[0] = Resources.Load("Textures/Drum/not select2") as Texture;
		_overTexture[1] = Resources.Load("Textures/Character/Heads/Character 1/Brown Hair Selected") as Texture;
		_overTexture[2] = Resources.Load("Textures/Character/Heads/Character 2/Black Hair Selected") as Texture;
		_overTexture[3] = Resources.Load("Textures/Character/Heads/Character 3/Blonde Hair Selected") as Texture;
		_overTexture[4] = Resources.Load("Textures/Character/Heads/Character 4/Red Hair Selected") as Texture;

		_deadTexture = new Texture[_numOfOptions];
		_deadTexture[0] = Resources.Load("Textures/Drum/not select") as Texture;
		_deadTexture[1] = Resources.Load("Textures/Character/Heads/Character 1/Brown Hair Dead") as Texture;
		_deadTexture[2] = Resources.Load("Textures/Character/Heads/Character 2/Black Hair Dead") as Texture;
		_deadTexture[3] = Resources.Load("Textures/Character/Heads/Character 3/Blonde Hair Dead") as Texture;
		_deadTexture[4] = Resources.Load("Textures/Character/Heads/Character 4/Red Hair Dead") as Texture;

		_isTextureLocked = new bool[_numOfOptions];
		_isTextureLocked[0] = false;
		_isTextureLocked[1] = false;
		_isTextureLocked[2] = false;
		_isTextureLocked[3] = false;
		_isTextureLocked[4] = false;

		_transform = new Transform[_numOfOptions];
		_transform[0] = transform.FindChild("Zero");
		_transform[1] = transform.FindChild("One");
		_transform[2] = transform.FindChild("Two");
		_transform[3] = transform.FindChild("Three");
		_transform[4] = transform.FindChild("Four");

		_renderer = new Renderer[_numOfOptions];
		for(int i=0; i<_numOfOptions; i++){
			_renderer[i] = _transform[i].GetComponent<Renderer>();
		}
	}
	public void UpdateButtonState(int index){
		for(int i=0; i<_renderer.Length; i++){
			if(i == index){
				_selectedIndex = i;
				if(!_isTextureLocked[i]){
					_transform[i].localScale = Constants.VOTE_OPTION_FACE_SCALE;
					//_renderer[i].material.mainTexture = _overTexture[i];
				}
			}else{
				_transform[i].localScale = Constants.VOTE_OPTION_FACE_DESELECT_SCALE;
				//_renderer[i].material.mainTexture = _texture[i];
			}
		}
	}
	public bool LockButtonState(){
		if(!_isTextureLocked[_selectedIndex]){
			_renderer[_selectedIndex].material.mainTexture = _overTexture[_selectedIndex];
			return true;
		}
		return false;
	}
	public void ResetButtonState(){
		if(_renderer != null){
			for(int i=0; i<_renderer.Length; i++){
				if(i != _selectedIndex){
					_transform[i].localScale = Constants.VOTE_OPTION_FACE_DESELECT_SCALE;
				}
				if(i > 0){ // Character starts from index 1
					// If the character is dead, lock the texture and change the default texture to dead
					if(CharacterManager.Instance.GetCharacter(i-1).IsCharacterDead()){
						_isTextureLocked[i] = true;
						_texture[i] = _deadTexture[i];
					}
				}
				_renderer[i].material.mainTexture = _texture[i];
			}
		}
	}
}
