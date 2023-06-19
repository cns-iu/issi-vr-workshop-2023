using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace AndreasBueckle.Assets.Scripts.Interaction
{
    public class SmoothTurnProvider : LocomotionProvider
    {
        // How much do we turn?
        [SerializeField] private float _turnSegment = 45f;

        // How long does it take to turn?
        [SerializeField] private float _turnTime = 3f;

        // Basic input - I'd recommend replacing with an input solution.
        //[SerializeField] private InputActionReference _rightTurnButton;
        //[SerializeField] private InputActionReference _leftTurnButton;
        [SerializeField] private InputHelpers.Button _rightTurnButton = InputHelpers.Button.PrimaryAxis2DRight;
        [SerializeField] private InputHelpers.Button _leftTurnButton = InputHelpers.Button.PrimaryAxis2DLeft;

        // List of the controllers we're going to use
        [SerializeField] private List<XRController> _controllers = new List<XRController>();

        // The amount we're turning to
        private float _targetTurnAmount = 0f;

        private void Update()
        {
            // Let's ask the locomotion system if we can move
            if (CanBeginLocomotion())
                CheckForInput();
        }

        private void CheckForInput()
        {
            foreach (XRController controller in _controllers)
            {
                _targetTurnAmount = CheckForTurn(controller);

                if (_targetTurnAmount != 0f)
                    TrySmoothTurn();

            }
        }

        // Check for input, this can be done cleaner with a more robust input solution
        private float CheckForTurn(XRController controller)
        {
            if (controller.inputDevice.IsPressed(_rightTurnButton, out bool rightPress))
            {
                if (rightPress)
                    return _turnSegment;
            }

            if (controller.inputDevice.IsPressed(_rightTurnButton, out bool leftPress))
            {
                if (leftPress)
                    return -_turnSegment;
            }

            return 0.0f;
        }

        private void TrySmoothTurn()
        {
            // Let's try turning with the amount we got
            StartCoroutine(TurnRoutine(_targetTurnAmount));

            // Since the value has been used, let's clear it out
            _targetTurnAmount = 0f;
        }

        private IEnumerator TurnRoutine(float turnAmount)
        {
            // We need to store this since we only want to pass the difference
            float previousTurnChange = 0f;

            // Record the whole time of the loop for proper lerp
            float elapsedTime = 0f;

            // Let the motion begin
            BeginLocomotion();

            while (elapsedTime <= _turnTime)
            {
                // How far are we into the lerp?
                float blend = elapsedTime / _turnTime;
                float turnChange = Mathf.Lerp(0, turnAmount, blend);

                // Figure out the difference and apply it
                float turnDifference = turnAmount - previousTurnChange;
                system.xrRig.RotateAroundCameraUsingRigUp(turnDifference);

                // Save the current amount we've moved, and add to elapsed time
                previousTurnChange = turnChange;
                elapsedTime += Time.deltaTime;

                // Yield or we're crashing
                yield return null;
            }

            // Let the motion end
            EndLocomotion();
        }
    }
}