using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedFox.Graphics3D.Skeletal
{
    /// <summary>
    /// A class to handle sampling an <see cref="SkeletonAnimation"/> at arbitrary frames or in a linear fashion.
    /// </summary>
    public class SkeletonAnimationSampler : AnimationSampler
    {
        public Skeleton Skeleton { get; set; }
        /// <summary>
        /// Gets or Sets the targets.
        /// </summary>
        public List<SkeletonAnimationTargetSampler> TargetSamplers { get; set; } = [];


        public SkeletonAnimationSampler(string name, SkeletonAnimation animation, Skeleton skeleton) : base(name, animation)
        {
            Skeleton = skeleton;
            skeleton.InitializeAnimationTransforms();

            foreach(var bone in skeleton.EnumerateHierarchy())
            {
                // Attempt to find a target
                var targetIndex = animation.Targets.FindIndex(x =>
                {
                    return x.BoneName.Equals(
                        bone.Name, 
                        StringComparison.CurrentCultureIgnoreCase);
                });

                SkeletonAnimationTarget? target = null;

                if (targetIndex != -1)
                    target = animation.Targets[targetIndex];

                TargetSamplers.Add(new(
                    this,
                    bone,
                    target,
                    animation.TransformType,
                    animation.TransformSpace));
            }
        }

        public SkeletonAnimationSampler(string name, SkeletonAnimation animation, Skeleton skeleton, AnimationPlayer player) : this(name, animation, skeleton)
        {
            player.AddLayer(this);
        }


        public void SetTransformType(TransformType type)
        {
            foreach (var targetSampler in TargetSamplers)
            {
                targetSampler.TransformType = type;
            }
        }

        public override void UpdateObjects()
        {
            foreach (var sampler in TargetSamplers)
            {
                sampler.Update();
            }
        }
    }
}
