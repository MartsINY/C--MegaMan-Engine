﻿using System;
using System.Xml.Linq;
using MegaMan.Common;
using MegaMan.Common.Entities;
using MegaMan.Common.Geometry;

namespace MegaMan.Engine
{
    [System.Diagnostics.DebuggerDisplay("Parent = {Parent.Name}, Position = {Position}")]
    public class PositionComponent : Component
    {
        public bool PersistOffScreen { get; set; }
        public PointF Position { get; private set; }
        public bool IsOffScreen
        {
            get
            {
                return !Parent.Screen.IsOnScreen(Position.X, Position.Y);
            }
        }

        public PositionComponent()
        {
            
        }

        public override Component Clone()
        {
            PositionComponent copy = new PositionComponent {PersistOffScreen = this.PersistOffScreen};
            return copy;
        }

        public override void Start(IGameplayContainer container)
        {
            container.GameCleanup += Update;
        }

        public override void Stop(IGameplayContainer container)
        {
            container.GameCleanup -= Update;
        }

        public override void Message(IGameMessage msg)
        {
            
        }

        public void SetPosition(float x, float y)
        {
            SetPosition(new PointF(x, y));
        }

        public void SetPosition(PointF pos)
        {
            // fix float errors by rounding
            pos.X = (float)Math.Round(pos.X, 3);
            pos.Y = (float)Math.Round(pos.Y, 3);
            Position = pos;
        }

        protected override void Update()
        {
            if (!PersistOffScreen && IsOffScreen && Parent.Name != "Player")
            {
                Parent.Remove();
                return;
            }
        }

        public override void RegisterDependencies(Component component)
        {
            
        }

        public void Offset(float x, float y)
        {
            Position = new PointF(Position.X + x, Position.Y + y);
        }

        internal void LoadInfo(PositionComponentInfo info)
        {
            PersistOffScreen = info.PersistOffscreen;
        }

        public override void LoadXml(XElement node)
        {
            throw new NotSupportedException("Should not call LoadXml for position component anymore.");
        }

        public static Effect ParseEffect(XElement child)
        {
            Effect action = entity => { };
            foreach (XElement prop in child.Elements())
            {
                switch (prop.Name.LocalName)
                {
                    case "X":
                        action += ParsePositionBehavior(prop, Axis.X);
                        break;

                    case "Y":
                        action += ParsePositionBehavior(prop, Axis.Y);
                        break;
                }
            }
            return action;
        }

        public static Effect ParsePositionBehavior(XElement prop, Axis axis)
        {
            Effect action = e => { };

            XAttribute baseAttr = prop.Attribute("base");
            if (baseAttr != null)
            {
                string base_str = baseAttr.Value;
                if (base_str == "Inherit")
                {
                    action = entity =>
                    {
                        PositionComponent pos = entity.GetComponent<PositionComponent>();
                        if (pos != null && entity.Parent != null)
                        {
                            PositionComponent parentPos = entity.Parent.GetComponent<PositionComponent>();
                            if (parentPos != null)
                            {
                                pos.SetPosition(axis == Axis.X
                                    ? new PointF(parentPos.Position.X, pos.Position.Y)
                                    : new PointF(pos.Position.X, parentPos.Position.Y));
                            }
                        }
                    };
                }
                else
                {
                    float baseVal = prop.TryAttribute<float>("base");
                    if (axis == Axis.X) action = entity =>
                    {
                        PositionComponent pos = entity.GetComponent<PositionComponent>();
                        if (pos != null) pos.SetPosition(new PointF(baseVal, pos.Position.Y));
                    };
                    else action = entity =>
                    {
                        PositionComponent pos = entity.GetComponent<PositionComponent>();
                        if (pos != null) pos.SetPosition(new PointF(pos.Position.X, baseVal));
                    };
                }
            }

            if (prop.Attribute("offset") != null)
            {
                float offset = prop.TryAttribute<float>("offset");
                XAttribute offdirattr = prop.RequireAttribute("direction");
                if (offdirattr.Value == "Inherit")
                {
                    action += entity =>
                    {
                        PositionComponent pos = entity.GetComponent<PositionComponent>();
                        if (pos != null && entity.Parent != null)
                        {
                            Direction offdir = entity.Parent.Direction;
                            switch (offdir)
                            {
                                case Direction.Down: pos.SetPosition(new PointF(pos.Position.X, pos.Position.Y + offset)); break;
                                case Direction.Up: pos.SetPosition(new PointF(pos.Position.X, pos.Position.Y - offset)); break;
                                case Direction.Left: pos.SetPosition(new PointF(pos.Position.X - offset, pos.Position.Y)); break;
                                case Direction.Right: pos.SetPosition(new PointF(pos.Position.X + offset, pos.Position.Y)); break;
                            }
                        }
                    };
                }
                else if (offdirattr.Value == "Input")
                {
                    action += entity =>
                    {
                        PositionComponent pos = entity.GetComponent<PositionComponent>();
                        InputComponent input = entity.GetComponent<InputComponent>();
                        if (input != null && pos != null)
                        {
                            if (axis == Axis.Y)
                            {
                                if (input.Down) pos.SetPosition(new PointF(pos.Position.X, pos.Position.Y + offset));
                                else if (input.Up) pos.SetPosition(new PointF(pos.Position.X, pos.Position.Y - offset));
                            }
                            else
                            {
                                if (input.Left) pos.SetPosition(new PointF(pos.Position.X - offset, pos.Position.Y));
                                else if (input.Right || (!input.Up && !input.Down)) pos.SetPosition(new PointF(pos.Position.X + offset, pos.Position.Y));
                            }
                        }
                    };
                }
                else
                {
                    Direction offdir;
                    try
                    {
                        offdir = (Direction)Enum.Parse(typeof(Direction), offdirattr.Value, true);
                    }
                    catch
                    {
                        throw new GameXmlException(offdirattr, "Position offset direction was not valid!");
                    }
                    switch (offdir)
                    {
                        case Direction.Left: action += entity =>
                        {
                            PositionComponent pos = entity.GetComponent<PositionComponent>();
                            if (pos != null) pos.SetPosition(new PointF(pos.Position.X - offset, pos.Position.Y));
                        };
                            break;
                        case Direction.Right: action += entity =>
                        {
                            PositionComponent pos = entity.GetComponent<PositionComponent>();
                            if (pos != null) pos.SetPosition(new PointF(pos.Position.X + offset, pos.Position.Y));
                        };
                            break;
                        case Direction.Down: action += entity =>
                        {
                            PositionComponent pos = entity.GetComponent<PositionComponent>();
                            if (pos != null) pos.SetPosition(new PointF(pos.Position.X, pos.Position.Y + offset));
                        };
                            break;
                        case Direction.Up: action += entity =>
                        {
                            PositionComponent pos = entity.GetComponent<PositionComponent>();
                            if (pos != null) pos.SetPosition(new PointF(pos.Position.X, pos.Position.Y - offset));
                        };
                            break;
                    }
                }
            }
            return action;
        }
    }
}
