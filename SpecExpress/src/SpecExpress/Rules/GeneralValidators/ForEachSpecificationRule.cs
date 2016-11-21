﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using SpecExpress.Util;

namespace SpecExpress.Rules.GeneralValidators
{

    public class ForEachSpecificationRule<T, TProperty, TCollectionType, TSpecification> : ForEachSpecificationRule<T, TProperty, TCollectionType> where TSpecification : Validates<TCollectionType>, new()
    {
        public ForEachSpecificationRule()
        {

        }

        public ForEachSpecificationRule(string itemName)
            : base(itemName)
        {

        }

        public override bool Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            //Resolve the Specification
            Specification = specificationContainer.TryGetSpecification<TSpecification>() as Validates<TCollectionType> ??
                       specificationContainer.DependencyContainer.Resolve<TSpecification>();

            return base.Validate(context, specificationContainer, notification);
        }

    }

    public class ForEachSpecificationRule<T, TProperty, TCollectionType> : RuleValidator<T, TProperty>
    {
        protected string ItemName;
        private SpecificationBase _specificationBase;
        protected Validates<TCollectionType> Specification;

        /// <summary>
        /// Validate using designated specification
        /// </summary>
        /// <param name="specification"></param>
        public ForEachSpecificationRule(Validates<TCollectionType> specification)
        {
            Specification = specification;
        }

        public ForEachSpecificationRule(Validates<TCollectionType> specification, string itemName)
            : this(specification)
        {
            ItemName = itemName;
        }

        /// <summary>
        /// Validation Property with default Specification from Registry
        /// </summary>
        public ForEachSpecificationRule()
        {
        }

        public ForEachSpecificationRule(string itemName)
            : this()
        {
            ItemName = itemName;
        }

        public override bool Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            if (Specification == null)
            {
                _specificationBase = specificationContainer.GetSpecification<TCollectionType>();
            }
            else
            {
                _specificationBase = Specification;
            }

            ValidationResult collectionValidationResult = null;

            //Check if the Collection is null/default
            if (context.PropertyValue.IsNullOrDefault())
            {
                return true;
            }

            var itemsNestedValidationResult = new List<ValidationResult>();

            var propertyEnumerable = ((IEnumerable)(context.PropertyValue));

            if (propertyEnumerable == null)
            {
                throw new ArgumentException("Property must be IEnumerable");
            }

            int index = 1;
            foreach (var item in propertyEnumerable)
            {
                var innerNotfication = new ValidationNotification();
                _specificationBase.Validate(item, specificationContainer, innerNotfication);

                if (innerNotfication.Errors.Any())
                {
                    var propertyName = String.IsNullOrEmpty(ItemName) ? item.GetType().Name : ItemName;

                    Message = String.Format("{0} {1} in {2} is invalid.", propertyName, index, context.PropertyName);

                    var childContext = new RuleValidatorContext(item, propertyName, item, context.Level,
                                                                item.GetType() as MemberInfo, context);

                    var itemError = ValidationResultFactory.Create(this, childContext, new List<object>(), MessageKey);

                    //var itemError = ValidationResultFactory.Create(this, context, Parameters, MessageKey);
                    itemError.NestedValidationResults = innerNotfication.Errors;
                    itemsNestedValidationResult.Add(itemError);
                }
                index++;
            }

            if (itemsNestedValidationResult.Any())
            {
                //Errors were found on at least one item in the collection to return a ValidationResult for the Collection property
                Message = "{PropertyName} is invalid.";

                collectionValidationResult = ValidationResultFactory.Create(this, context, new List<object>(), MessageKey);
                collectionValidationResult.NestedValidationResults = itemsNestedValidationResult;
                notification.Errors.Add(collectionValidationResult);
                return false;
            }
            else
            {
                return true;
            }
        }
    }



    public class ForEachSpecificationRuleUntyped<T, TProperty> : RuleValidator<T, TProperty>
    {
        protected string ItemName;

        public ForEachSpecificationRuleUntyped(string itemName)
        {
            ItemName = itemName;
        }

        /// <summary>
        /// Validation Property with default Specification from Registry
        /// </summary>
        public ForEachSpecificationRuleUntyped()
        {
        }



        public override bool Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            ValidationResult collectionValidationResult = null;

            //Check if the Collection is null/default
            if (context.PropertyValue.IsNullOrDefault())
            {
                return true;
            }


            var itemsNestedValidationResult = new List<ValidationResult>();

            var propertyEnumerable = ((IEnumerable)(context.PropertyValue));

            if (propertyEnumerable == null)
            {
                throw new ArgumentException("Property must be IEnumerable");
            }

            int index = 1;
            foreach (var item in propertyEnumerable)
            {
                if (item == null)
                {
                    throw new NullReferenceException(string.Format("One of the items is null in the list {0} of the instance {1}", context.PropertyName, context.Instance.GetType().FullName));
                }
                
                var innerNotfication = new ValidationNotification();

                Type objectType = item.GetType();

                var specification = specificationContainer.GetSpecification(objectType);
                if (specification == null)
                {
                    continue;
                }

                if (!specification.Validate(item, specificationContainer, innerNotfication))
                {
                    var propertyName = String.IsNullOrEmpty(ItemName) ? item.GetType().Name : ItemName;

                    Message = String.Format("{0} {1} in {2} is invalid.", propertyName, index, context.PropertyName);

                    var childContext = new RuleValidatorContext(item, propertyName, item, context.Level,
                                                                item.GetType() as MemberInfo, context);

                    var itemError = ValidationResultFactory.Create(this, childContext, new List<object>(), MessageKey);

                    //var itemError = ValidationResultFactory.Create(this, context, Parameters, MessageKey);
                    itemError.NestedValidationResults = innerNotfication.Errors;
                    itemsNestedValidationResult.Add(itemError);
                }
                index++;
            }

            if (itemsNestedValidationResult.Any())
            {
                //Errors were found on at least one item in the collection to return a ValidationResult for the Collection property
                Message = "{PropertyName} is invalid.";
                collectionValidationResult = ValidationResultFactory.Create(this, context, new List<object>(), MessageKey);
                collectionValidationResult.NestedValidationResults = itemsNestedValidationResult;
                notification.Errors.Add(collectionValidationResult);
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
