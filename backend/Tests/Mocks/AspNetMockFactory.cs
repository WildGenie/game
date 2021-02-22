using System;
using Moq;

namespace Tests.Mocks
{
	public static class AspNetMockFactory
	{
		public static Mock<IServiceProvider> ServiceProvider()
		{
			return new Mock<IServiceProvider>();
		}
	}
}