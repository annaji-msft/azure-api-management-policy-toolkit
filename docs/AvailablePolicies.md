# Available Policies

The Project is in the development stage.
That means that not all policies are implemented yet.
In this document, you can find a list of implemented policies. For policy details, see the Azure API Management [policy reference](https://learn.microsoft.com/azure/api-management/api-management-policies).

#### :white_check_mark: Implemented policies

* authentication-basic
* authentication-certificate
* authentication-managed-identity
* azure-openai-emit-token-metric
* azure-openai-semantic-cache-lookup
* azure-openai-semantic-cache-store
* base
* cache-lookup
* cache-lookup-value
* cache-remove-value
* cache-store
* cache-store-value
* check-header
* choose
* cors
* cross-domain
* emit-metric
* find-and-replace
* forward-request
* get-authorization-context
* include-fragment
* invoke-dapr-binding
* ip-filter
* json-to-xml
* jsonp
* limit-concurrency
* llm-emit-token-metric
* llm-semantic-cache-lookup
* llm-semantic-cache-store
* llm-token-limit
* log-to-eventhub
* mock-response
* proxy
* publish-to-dapr
* quota
* quota-by-key
* rate-limit
* rate-limit-by-key
* redirect-content-urls
* retry
* return-response
* rewrite-uri
* send-one-way-request
* send-request
* set-backend-service
* set-backend-service (Dapr)
* set-body
* set-header
* set-method
* set-query-parameter
* set-status
* set-variable
* trace
* validate-azure-ad-token
* validate-client-certificate
* validate-content
* validate-headers
* validate-jwt
* validate-odata-request
* validate-parameters
* validate-status-code
* wait
* xml-to-json
* xsl-transform

Policies not listed here are not implemented yet, we are curious to know which [ones you'd like to use and are happy to review contributions](./../CONTRIBUTING.md).

## InlinePolicy

InlinePolicy is a workaround until all the policies are implemented.
It allows you to include policy not implemented yet to the document.

```csharp
c.InlinePolicy("<set-backend-service base-url=\"https://internal.contoso.example\" />");
```
