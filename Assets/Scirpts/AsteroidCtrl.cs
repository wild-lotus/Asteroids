﻿using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

using Random = UnityEngine.Random;

namespace CgfGames
{
	public interface IAsteroidCtrl
	{
		int Size { get; }

		event Action<IAsteroidCtrl, List<IAsteroidCtrl>> DestroyedEvent;

		void Destroyed ();
	}

	[Serializable]
	public class AsteroidCtrl : IAsteroidCtrl
	{
		#region Constants
		//======================================================================

		public const int MAX_SIZE = 2;

		#endregion

		#region Public fields & properties
		//======================================================================

		public int Size { get; private set; }

		#endregion

		#region Events
		//======================================================================

		public event Action<IAsteroidCtrl, List<IAsteroidCtrl>> DestroyedEvent;
		
		#endregion

		#region External references
		//======================================================================

		public AsteroidView View;

		#endregion

		#region Init
		//======================================================================

		public AsteroidCtrl (IAsteroidView view, int size)
		{
			this.View = view as AsteroidView;
			this.Size = size;
			this.View.HitEvent += this.Destroyed;
		}

		#endregion

		#region Public methods
		//======================================================================

		public void Destroyed ()
 		{
			List<IAsteroidCtrl> asteroidCtrlList = null;
			if (this.Size == MAX_SIZE) {
				this.View.TrySpawnPowerup ();
			}
			if (this.Size > 0) {
				asteroidCtrlList = this.SpawnChildren ();
			}
			if (DestroyedEvent != null) {
				this.DestroyedEvent (this, asteroidCtrlList);
			}
			this.View.Destroyed ();
			this.View.HitEvent -= this.Destroyed;
		}

		private List<IAsteroidCtrl> SpawnChildren ()
		{
			int childSize = this.Size - 1;
			return this.View.SpawnChildren (childSize)
				.Select ((view) => 
					new AsteroidCtrl (view, childSize) as IAsteroidCtrl)
				.ToList ();
		}

		#endregion
	}
}