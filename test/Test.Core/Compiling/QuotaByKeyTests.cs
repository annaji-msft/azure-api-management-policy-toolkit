using System.Xml.Linq;
using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Compiling.Policy;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using FluentAssertions;

namespace Azure.ApiManagement.PolicyToolkit.Compiling;

[TestClass]
public class QuotaByKeyTests
{
    [TestMethod]
    public void ShouldCompileQuotaByKeyPolicy()
    {
        var code =
            """
            [Document]
            public class PolicyDocument : IDocument
            {
                public void Inbound(IInboundContext context)
                {
                    context.QuotaByKey(new QuotaConfig
                    {
                        Calls = 100,
                        RenewalPeriod = 3600,
                        CounterKey = "user",
                        Apis = new[]
                        {
                            new ApiQuota
                            {
                                Name = "api1",
                                Calls = 50,
                                Operations = new[]
                                {
                                    new OperationQuota
                                    {
                                        Name = "operation1",
                                        Calls = 25
                                    }
                                }
                            }
                        }
                    });
                }
            }
            """;
        var expectedXml =
            """
            <policies>
                <inbound>
                    <quota-by-key calls="100" renewal-period="3600" counter-key="user">
                        <api name="api1" calls="50">
                            <operation name="operation1" calls="25" />
                        </api>
                    </quota-by-key>
                </inbound>
            </policies>
            """;

        code.CompileDocument().Should().BeSuccessful().And.DocumentEquivalentTo(expectedXml);
    }

    [TestMethod]
    public void ShouldReportErrorWhenCallsAndBandwidthAreNotDefined()
    {
        var code =
            """
            [Document]
            public class PolicyDocument : IDocument
            {
                public void Inbound(IInboundContext context)
                {
                    context.QuotaByKey(new QuotaConfig
                    {
                        RenewalPeriod = 3600,
                        CounterKey = "user"
                    });
                }
            }
            """;

        var result = code.CompileDocument();
        result.Should().HaveDiagnostics().And.HaveError("APIM9997");
    }

    [TestMethod]
    public void ShouldReportErrorWhenRenewalPeriodIsNotDefined()
    {
        var code =
            """
            [Document]
            public class PolicyDocument : IDocument
            {
                public void Inbound(IInboundContext context)
                {
                    context.QuotaByKey(new QuotaConfig
                    {
                        Calls = 100,
                        CounterKey = "user"
                    });
                }
            }
            """;

        var result = code.CompileDocument();
        result.Should().HaveDiagnostics().And.HaveError("APIM2006");
    }

    [TestMethod]
    public void ShouldReportErrorWhenCounterKeyIsNotDefined()
    {
        var code =
            """
            [Document]
            public class PolicyDocument : IDocument
            {
                public void Inbound(IInboundContext context)
                {
                    context.QuotaByKey(new QuotaConfig
                    {
                        Calls = 100,
                        RenewalPeriod = 3600
                    });
                }
            }
            """;

        var result = code.CompileDocument();
        result.Should().HaveDiagnostics().And.HaveError("APIM2006");
    }

    [TestMethod]
    public void ShouldReportErrorWhenApiNameAndIdAreNotDefined()
    {
        var code =
            """
            [Document]
            public class PolicyDocument : IDocument
            {
                public void Inbound(IInboundContext context)
                {
                    context.QuotaByKey(new QuotaConfig
                    {
                        Calls = 100,
                        RenewalPeriod = 3600,
                        CounterKey = "user",
                        Apis = new[]
                        {
                            new ApiQuota
                            {
                                Calls = 50
                            }
                        }
                    });
                }
            }
            """;

        var result = code.CompileDocument();
        result.Should().HaveDiagnostics().And.HaveError("APIM9996");
    }

    [TestMethod]
    public void ShouldReportErrorWhenApiCallsAndBandwidthAreNotDefined()
    {
        var code =
            """
            [Document]
            public class PolicyDocument : IDocument
            {
                public void Inbound(IInboundContext context)
                {
                    context.QuotaByKey(new QuotaConfig
                    {
                        Calls = 100,
                        RenewalPeriod = 3600,
                        CounterKey = "user",
                        Apis = new[]
                        {
                            new ApiQuota
                            {
                                Name = "api1"
                            }
                        }
                    });
                }
            }
            """;

        var result = code.CompileDocument();
        result.Should().HaveDiagnostics().And.HaveError("APIM9996");
    }

    [TestMethod]
    public void ShouldReportErrorWhenOperationNameAndIdAreNotDefined()
    {
        var code =
            """
            [Document]
            public class PolicyDocument : IDocument
            {
                public void Inbound(IInboundContext context)
                {
                    context.QuotaByKey(new QuotaConfig
                    {
                        Calls = 100,
                        RenewalPeriod = 3600,
                        CounterKey = "user",
                        Apis = new[]
                        {
                            new ApiQuota
                            {
                                Name = "api1",
                                Calls = 50,
                                Operations = new[]
                                {
                                    new OperationQuota()
                                }
                            }
                        }
                    });
                }
            }
            """;

        var result = code.CompileDocument();
        result.Should().HaveDiagnostics().And.HaveError("APIM9996");
    }

    [TestMethod]
    public void ShouldReportErrorWhenOperationCallsAndBandwidthAreNotDefined()
    {
        var code =
            """
            [Document]
            public class PolicyDocument : IDocument
            {
                public void Inbound(IInboundContext context)
                {
                    context.QuotaByKey(new QuotaConfig
                    {
                        Calls = 100,
                        RenewalPeriod = 3600,
                        CounterKey = "user",
                        Apis = new[]
                        {
                            new ApiQuota
                            {
                                Name = "api1",
                                Calls = 50,
                                Operations = new[]
                                {
                                    new OperationQuota
                                    {
                                        Name = "operation1"
                                    }
                                }
                            }
                        }
                    });
                }
            }
            """;

        var result = code.CompileDocument();
        result.Should().HaveDiagnostics().And.HaveError("APIM9996");
    }
}
