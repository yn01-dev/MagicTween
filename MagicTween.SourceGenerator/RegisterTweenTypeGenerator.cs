﻿using Microsoft.CodeAnalysis;

namespace MagicTween.Generator
{
    [Generator]
    public sealed class RegisterTweenTypeGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var compilation = context.Compilation;
            var attributes = compilation.Assembly.GetAttributes();
            foreach (var attribute in attributes)
            {
                var attributeClassName = attribute.AttributeClass.Name;

                if (attributeClassName == "MagicTween.RegisterTweenTypeAttribute" ||
                    attributeClassName == "MagicTween.RegisterTweenType" ||
                    attributeClassName == "RegisterTweenType" ||
                    attributeClassName == "RegisterTweenTypeAttribute")
                {
                    var valueType = (INamedTypeSymbol)attribute.ConstructorArguments[0].Value;
                    var optionsType = (INamedTypeSymbol)attribute.ConstructorArguments[1].Value;
                    var pluginType = (INamedTypeSymbol)attribute.ConstructorArguments[2].Value;

                    var valueTypeFullName = valueType.ContainingNamespace.IsGlobalNamespace ? valueType.Name : valueType.ContainingNamespace + "." + valueType.Name;
                    var optionsTypeFullName = valueType.ContainingNamespace.IsGlobalNamespace ? optionsType.Name : optionsType.ContainingNamespace + "." + optionsType.Name;
                    var pluginTypeFullName = valueType.ContainingNamespace.IsGlobalNamespace ? pluginType.Name : pluginType.ContainingNamespace + "." + pluginType.Name;
                    context.AddSource($"__{valueTypeFullName}_tween_type.g.cs",
$@"
// <auto-generated/>

using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using MagicTween.Core;
using MagicTween.Core.Components;
using MagicTween.Core.Systems;
using MagicTween.Plugins;
using MagicTween;

[assembly: RegisterGenericComponentType(typeof(TweenValue<global::{valueTypeFullName}>))]
[assembly: RegisterGenericComponentType(typeof(TweenStartValue<global::{valueTypeFullName}>))]
[assembly: RegisterGenericComponentType(typeof(TweenEndValue<global::{valueTypeFullName}>))]
[assembly: RegisterGenericComponentType(typeof(TweenOptions<global::{optionsTypeFullName}>))]
[assembly: RegisterGenericComponentType(typeof(TweenDelegates<global::{valueTypeFullName}>))]
[assembly: RegisterGenericComponentType(typeof(TweenDelegatesNoAlloc<global::{valueTypeFullName}>))]

namespace MagicTween.Generated{(valueType.ContainingNamespace.IsGlobalNamespace ? string.Empty : "." + valueType.ContainingNamespace.Name)}
{{
    partial class {valueType.Name}TweenSystem : StandardTweenSystemBase<global::{valueTypeFullName}, global::{optionsTypeFullName}, global::{pluginTypeFullName}> {{ }}
    partial class {valueType.Name}TweenDelegateTranslationSystem : TweenDelegateTranslationSystemBase<global::{valueTypeFullName}, global::{optionsTypeFullName}, global::{pluginTypeFullName}> {{ }}
}}
");
                }
            }
        }

        public void Initialize(GeneratorInitializationContext context) { }
    }
}

