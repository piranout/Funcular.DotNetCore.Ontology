#region File info
// *********************************************************************************************************
// Funcular.Ontology.Archetypes.Createable.cs
// 
// *********************************************************************************************************
// LICENSE: The MIT License (MIT)
// *********************************************************************************************************
// Copyright (c) 2010-2017 Funcular Labs and Paul Smith
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
// *********************************************************************************************************
#endregion


#region Usings
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Funcular.DotNetCore.Ontology.Annotations;
#endregion


namespace Funcular.DotNetCore.Ontology.Archetypes
{
    /// <summary>
    /// This is the base abstract class from which every entity ultimately derives.
    /// 
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public abstract class Createable<TId> : ICreateable<TId>  where TId : IEquatable<TId>
    {
        #region Nonpublic fields
        protected TId _createdBy;
        protected TId _id;
        protected DateTime _dateCreatedUtc = DateTime.UtcNow;
        #endregion


        #region Constructors
        /// <summary>
        /// If you override this constructor, you will need to re-implement
        /// the identity assignment function.
        /// </summary>
        protected Createable(bool suppressIdGeneration = false)
        {
            if (suppressIdGeneration == false && IdentityFunction != null)
                this._id = IdentityFunction();
        }
        #endregion


        #region Implementation of IIdentity

        public virtual TId Id
        {
            get => _id;
            set
            {
                if (EqualityComparer<TId>.Default.Equals(_id, value))
                {
                    return;
                }
                _id = value;
                OnPropertyChanged();
            }
        }

        #endregion


        #region Implementation of ICreateable

        /// <summary>
        /// This property should not change in the regular course of 
        /// business, which is why PropertyChanged is not implemented here.
        /// The setter will be called by framework methods though, making
        /// it impractical to make the property read-only.
        /// </summary>
        public virtual DateTime DateCreatedUtc
        {
            get => _dateCreatedUtc;
            set
            {
                _dateCreatedUtc = value;
                OnPropertyChanged();
            }
        }

        public virtual TId CreatedBy
        {
            get => this._createdBy;
            set
            {
                if (EqualityComparer<TId>.Default.Equals(_createdBy, value))
                {
                    return;
                }
                this._createdBy = value; 
                OnPropertyChanged();
            }
        }

        #endregion


        #region Identity assignment method
        /// <summary>
        ///     Set this once in your app domain and ID assignment is done
        ///     for all entities.e.g., calling
        /// </summary>
        /// <example>
        /// // Assuming you have an Id generator instance...
        /// Createable.IdentityFunction = () => _generator.NewId();
        /// // Make sure this function does not throw exceptions.
        /// </example>
        public static Func<TId> IdentityFunction { get; set; }
        #endregion


        #region OnPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// This invocator should be called by derived classes in
        /// their property setters. 
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}