using UnityEngine;
using System.Collections;

public class VoteFour : MonoBehaviour {

	private Transform[] _transform;
	private Renderer[] _renderer;
	private Texture[] _texture, _overTexture;
	private int _selectedIndex;

	void Start(){	
		// Initialise voting prefab here
		int _numOfOptions = 4;
		_texture = new Texture[_numOfOptions];
		_texture[0] = Resources.Load("Textures/Drum/0") as Texture;
		_texture[1] = Resources.Load("Textures/Drum/1") as Texture;
		_texture[2] = Resources.Load("Textures/Drum/2") as Texture;
		_texture[3] = Resources.Load("Textures/Drum/3") as Texture;

		_overTexture = new Texture[_numOfOptions];
		_overTexture[0] = Resources.Load("Textures/Drum/0c") as Texture;
		_overTexture[1] = Resources.Load("Textures/Drum/1c") as Texture;
		_overTexture[2] = Resources.Load("Textures/Drum/2c") as Texture;
		_overTexture[3] = Resources.Load("Textures/Drum/3c") as Texture;

		_transform = new Transform[_numOfOptions];
		_transform[0] = transform.FindChild("Zero");
		_transform[1] = transform.FindChild("One");
		_transform[2] = transform.FindChild("Two");
		_transform[3] = transform.FindChild("Three");

		_renderer = new Renderer[_numOfOptions];
		for(int i=0; i<_numOfOptions; i++){
			_renderer[i] = _transform[i].GetComponent<Renderer>();
		}
	}
	public void UpdateButtonState(int index){
		for(int i=0; i<_renderer.Length; i++){
			if(i == index){
				_selectedIndex = i;
				_transform[i].localScale = Constants.VOTE_OPTION_SELECT_SCALE;
				//_renderer[i].material.mainTexture = _overTexture[i];
			}else{
				_transform[i].localScale = Constants.VOTE_OPTION_DESELECT_SCALE;
				//_renderer[i].material.mainTexture = _texture[i];
			}
		}
	}
	public bool LockButtonState(){
		_renderer[_selectedIndex].material.mainTexture = _overTexture[_selectedIndex];
		return true;
	}
	public void ResetButtonState(){
		if(_renderer != null){
			for(int i=0; i<_renderer.Length; i++){
				_renderer[i].material.mainTexture = _texture[i];
				if(i != _selectedIndex){
					_transform[i].localScale = Constants.VOTE_OPTION_DESELECT_SCALE;
				}
			}	
		}
	}
}
