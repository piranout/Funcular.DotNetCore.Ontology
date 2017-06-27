#region File info
// *********************************************************************************************************
// Funcular.Ontology>Funcular.Ontology>Createable.cs
// Created: 2015-07-01 2:16 PM
// Updated: 2015-07-01 2:50 PM
// By: Paul Smith 
// 
// *********************************************************************************************************
// LICENSE: The MIT License (MIT)
// *********************************************************************************************************
// Copyright (c) 2010-2015 <copyright holders>
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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Funcular.DotNetCore.Ontology.Annotations;

#endregion


namespace Funcular.DotNetCore.Ontology.Archetypes
{
    public abstract class Createable<TId> : ICreateable<TId>
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
        protected Createable()
        {
            if (IdentityFunction != null)
                this._id = IdentityFunction();
        }
        #endregion


        #region Implementation of IIdentity

        public virtual TId Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        #endregion


        #region Implementation of ICreateable

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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}