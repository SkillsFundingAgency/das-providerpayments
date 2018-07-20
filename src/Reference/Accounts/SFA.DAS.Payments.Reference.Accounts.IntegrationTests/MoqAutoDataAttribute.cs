﻿using System;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;

namespace SFA.DAS.Payments.Reference.Accounts.IntegrationTests
{
    public class MoqAutoDataAttribute : AutoDataAttribute
    {
        public MoqAutoDataAttribute() : base(FixtureBuilder.FixtureFactory)
        { }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class MoqInlineAutoDataAttribute : InlineAutoDataAttribute
    {
        public MoqInlineAutoDataAttribute(params object[] arguments)
            : base(FixtureBuilder.FixtureFactory, arguments)
        {
        }
    }

    static class FixtureBuilder
    {
        public static IFixture FixtureFactory()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());

            return fixture;
        }
    }
}