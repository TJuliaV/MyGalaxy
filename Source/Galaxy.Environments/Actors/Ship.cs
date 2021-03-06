﻿#region using

using System;
using System.Diagnostics;
using System.Windows;
using Galaxy.Core.Actors;
using Galaxy.Core.Environment;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

#endregion

namespace Galaxy.Environments.Actors
{
  public class Ship : DethAnimationActor
  {
    #region Constant

    protected const int MaxSpeed = 1;
    protected const long StartFlyMs = 1000;

    #endregion

    #region Private fields

    protected bool m_flying;
    protected Stopwatch m_flyTimer;

    #endregion

      public StyleOfFly m_styleoffly;

    #region Constructors

    public Ship(ILevelInfo info):base(info)
    {
      Width = 30;
      Height = 30;
      ActorType = ActorType.Enemy;
    }

    #endregion

    #region Overrides

    public override void Update()
    {
      base.Update();

      if (!IsAlive)
        return;

      if (!m_flying)
      {
        if (m_flyTimer.ElapsedMilliseconds <= StartFlyMs) return;

        m_flyTimer.Stop();
        m_flyTimer = null;
        h_changePosition();
        m_flying = true;
      }
      else
      {
        h_changePosition();
      }
    }

    #endregion

    #region Overrides

    public override void Load()
    {
      Load(@"Assets\ship.png");
      if (m_flyTimer == null)
      {
        m_flyTimer = new Stopwatch();
        m_flyTimer.Start();
      }
    }

    #endregion

    #region Private methods

    private void h_changePosition()
    {
        Point playerPosition = Info.GetPlayerPosition();
        Size levelSize = Info.GetLevelSize();

        int newX = 0;
        int newY = 0;

        if (m_styleoffly == StyleOfFly.Vector)
        {
              Vector distance = new Vector(playerPosition.X - Position.X, playerPosition.Y - Position.Y);
              double coef = distance.X / MaxSpeed;

              Vector movement = Vector.Divide(distance, coef);

              if(movement.X > levelSize.Width)
                movement = new Vector(levelSize.Width, movement.Y);

              if(movement.X < 0 || double.IsNaN(movement.X))
                movement = new Vector(0, movement.Y);

              if(movement.Y > levelSize.Height)
                movement = new Vector(movement.X, levelSize.Height);

              if(movement.Y < 0 ||  double.IsNaN(movement.Y))
                movement = new Vector(movement.X, 0);

            newX = (int) (Position.X + movement.X);
            newY = (int) (Position.Y + movement.Y);
        }

        if (m_styleoffly == StyleOfFly.Sin)
        {
            newX = Position.X + 1;
            newY = (int)(Position.Y + Math.Round(Math.Sin(Position.X / 40)));

        }
        if (m_styleoffly == StyleOfFly.Cos)
        {
            newX = Position.X - 1;
            newY = (int)(Position.Y + Math.Round(Math.Cos(Position.X / 50)));
        }
        if (m_styleoffly == StyleOfFly.Deth)
        {
            newX = Position.X;
            newY = Position.Y-2;
        }

      Position = new Point(newX, newY);
    }

    #endregion
  }
}
