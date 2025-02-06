using System.Collections.Generic;
using Base.Models;
using Cache;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core
{
    public class ShelfPresenter : MonoBehaviour
    {
        [Header("Model")] 
        [Inject] private CacheGame _cacheGame;
        [Inject] private CacheAudio _cacheAudio;
        
        [Header("View")] 
        [SerializeField] private List<BaseModels.ShelfElement> shelfSetup = new();
        [SerializeField] private UiInvisible panelRestart;
        [SerializeField] private Button buttonRestart;
        [SerializeField] private Transform dragContainer;
        
        [Header("Presenter Logic")] 
        private ShelfInstances _shelfInstances;
        private ShelfValidator _shelfValidator;
        private ShelfShuffler _shelfShuffler;
        private ShelfRestartUi _shelfRestartUi;

        private void Awake()
        {
            _shelfInstances = new ShelfInstances(_cacheGame, dragContainer);
            _shelfValidator = new ShelfValidator(_shelfInstances, _cacheAudio);
            _shelfShuffler = new ShelfShuffler(_shelfInstances, _cacheGame);
            _shelfRestartUi = new ShelfRestartUi(panelRestart, buttonRestart, _cacheGame, _cacheAudio);
        }

        private void Start()
        {
            _shelfInstances.BootShelves(shelfSetup);
            
            _shelfValidator.AddTriggersValidation();
            
            _shelfValidator.onFinishValidation += _shelfRestartUi.RestartPanel;
            
            _shelfRestartUi.onRestart += _shelfShuffler.ShuffleItems;
            
            _shelfShuffler.ShuffleItems();
        }
    }

}