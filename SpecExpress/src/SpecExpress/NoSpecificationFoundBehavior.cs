namespace SpecExpress
{
    using System;

    [Flags]
    public enum NoSpecificationFoundBehavior
    {
        None = 0,
        RaiseExceptionIfNoSpecificationsFound = 1,
        RaiseExceptionIfNoSpecificationsForTypeFound = 2
    }
}