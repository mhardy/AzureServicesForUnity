﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
namespace AzureServicesForUnity.QueryHelpers
{
    public class ODataQuery<T> : IQueryable<T>, IQueryable, IEnumerable<T>, IEnumerable, IOrderedQueryable<T>, IOrderedQueryable
    {
        QueryProviderBase provider;
        Expression expression;
        public ODataQuery(QueryProviderBase provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }
            this.provider = provider;
            this.expression = Expression.Constant(this);
        }
        public ODataQuery(QueryProviderBase provider, Expression expression)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            if (!typeof(IQueryable<T>).IsAssignableFrom(expression.Type))
            {
                throw new ArgumentOutOfRangeException("expression");
            }
            this.provider = provider;
            this.expression = expression;
        }
        Expression IQueryable.Expression
        {
            get { return this.expression; }
        }
        Type IQueryable.ElementType
        {
            get { return typeof(T); }
        }
        IQueryProvider IQueryable.Provider
        {
            get { return this.provider; }
        }
        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)this.provider.Execute(this.expression)).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.provider.Execute(this.expression)).GetEnumerator();
        }
        public override string ToString()
        {
            return this.provider.GetQueryText(this.expression);
        }
    }
}
