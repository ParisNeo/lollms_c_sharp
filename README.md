<p></p><h1 style="font-size: 2.2em; font-weight: bold;">User Guide for Lollms_Client_CS</h1><p></p>
<br>
<p></p><h2 style="font-size: 1.9em; font-weight: bold;">Overview</h2><p></p>
<br>
<p>The <b>Lollms_Client_CS</b> library provides a client for interacting with the LOLLMS API, enabling users to generate text, tokenize and detokenize prompts, manage templates, and handle various settings related to text generation. This guide will help you understand how to use the library effectively.</p>
<br>
<p></p><h2 style="font-size: 1.9em; font-weight: bold;">Getting Started</h2><p></p>
<br>
<p></p><h3 style="font-size: 1.6em; font-weight: bold;">Prerequisites</h3>
<ul><ol><li>.NET SDK installed on your machine.</li>
<li>Basic understanding of C# programming.</li>
<li>Access to a LOLLMS server.</li></ol></ul><p></p>
<br>
<p></p><h3 style="font-size: 1.6em; font-weight: bold;">Installation</h3>
<li>Clone or download the repository containing the <b>Lollms_Client_CS</b> code.</li>
<li>Add a reference to the project in your C# application.</li><p></p>
<br>
<p></p><h3 style="font-size: 1.6em; font-weight: bold;">Initialization</h3><p></p>
<br>
<p>To use the <b>LollmsClient</b>, you need to create an instance of it by providing the necessary parameters:</p>
<br>
<br>
<p>                </p><div class="code-block">
                    <div class="code-header">
                        <span class="language">csharp</span>
                        <button onclick="mr.copyCode(this)" class="copy-button">Copy</button>
                    </div>
                    <pre class="code-content"><code class="hljs language-csharp"><div class="code-line">
                    <span class="line-number">01</span>
                    <span class="line-content"><span class="hljs-keyword">var</span> client = <span class="hljs-keyword">new</span> LollmsClient(</span>
                 </div><div class="code-line">
                    <span class="line-number">02</span>
                    <span class="line-content">    hostAddress: <span class="hljs-string">"http://your-lollms-server.com"</span>,</span>
                 </div><div class="code-line">
                    <span class="line-number">03</span>
                    <span class="line-content">    modelName: <span class="hljs-string">"your-model-name"</span>,</span>
                 </div><div class="code-line">
                    <span class="line-number">04</span>
                    <span class="line-content">    ctxSize: <span class="hljs-number">4096</span>,</span>
                 </div><div class="code-line">
                    <span class="line-number">05</span>
                    <span class="line-content">    personality: <span class="hljs-number">-1</span>,</span>
                 </div><div class="code-line">
                    <span class="line-number">06</span>
                    <span class="line-content">    nPredict: <span class="hljs-number">4096</span>,</span>
                 </div><div class="code-line">
                    <span class="line-number">07</span>
                    <span class="line-content">    temperature: <span class="hljs-number">0.1</span>,</span>
                 </div><div class="code-line">
                    <span class="line-number">08</span>
                    <span class="line-content">    topK: <span class="hljs-number">50</span>,</span>
                 </div><div class="code-line">
                    <span class="line-number">09</span>
                    <span class="line-content">    topP: <span class="hljs-number">0.95</span>,</span>
                 </div><div class="code-line">
                    <span class="line-number">10</span>
                    <span class="line-content">    repeatPenalty: <span class="hljs-number">0.8</span>,</span>
                 </div><div class="code-line">
                    <span class="line-number">11</span>
                    <span class="line-content">    repeatLastN: <span class="hljs-number">40</span>,</span>
                 </div><div class="code-line">
                    <span class="line-number">12</span>
                    <span class="line-content">    seed: <span class="hljs-literal">null</span>,</span>
                 </div><div class="code-line">
                    <span class="line-number">13</span>
                    <span class="line-content">    nThreads: <span class="hljs-number">8</span>,</span>
                 </div><div class="code-line">
                    <span class="line-number">14</span>
                    <span class="line-content">    serviceKey: <span class="hljs-string">"your-service-key"</span>,</span>
                 </div><div class="code-line">
                    <span class="line-number">15</span>
                    <span class="line-content">    defaultGenerationMode: ELF_GENERATION_FORMAT.LOLLMS</span>
                 </div><div class="code-line">
                    <span class="line-number">16</span>
                    <span class="line-content">);</span>
                 </div></code></pre>
                </div><p></p>
<br>
<br>
<p></p><h3 style="font-size: 1.6em; font-weight: bold;">Loading Templates</h3><p></p>
<br>
<p>To load the message templates used for formatting the input and output, call the <b>LoadTemplateAsync</b> method:</p>
<br>
<br>
<p>                </p><div class="code-block">
                    <div class="code-header">
                        <span class="language">csharp</span>
                        <button onclick="mr.copyCode(this)" class="copy-button">Copy</button>
                    </div>
                    <pre class="code-content"><code class="hljs language-csharp"><div class="code-line">
                    <span class="line-number">01</span>
                    <span class="line-content"><span class="hljs-keyword">await</span> client.LoadTemplateAsync();</span>
                 </div></code></pre>
                </div><p></p>
<br>
<br>
<p></p><h3 style="font-size: 1.6em; font-weight: bold;">Updating Settings</h3><p></p>
<br>
<p>You can update the settings of the <b>LollmsClient</b> using a dictionary:</p>
<br>
<br>
<p>                </p><div class="code-block">
                    <div class="code-header">
                        <span class="language">csharp</span>
                        <button onclick="mr.copyCode(this)" class="copy-button">Copy</button>
                    </div>
                    <pre class="code-content"><code class="hljs language-csharp"><div class="code-line">
                    <span class="line-number">01</span>
                    <span class="line-content"><span class="hljs-keyword">var</span> settings = <span class="hljs-keyword">new</span> Dictionary&lt;<span class="hljs-built_in">string</span>, <span class="hljs-built_in">object</span>&gt;</span>
                 </div><div class="code-line">
                    <span class="line-number">02</span>
                    <span class="line-content">{</span>
                 </div><div class="code-line">
                    <span class="line-number">03</span>
                    <span class="line-content">    { <span class="hljs-string">"host_address"</span>, <span class="hljs-string">"http://new-lollms-server.com"</span> },</span>
                 </div><div class="code-line">
                    <span class="line-number">04</span>
                    <span class="line-content">    { <span class="hljs-string">"model_name"</span>, <span class="hljs-string">"new-model-name"</span> },</span>
                 </div><div class="code-line">
                    <span class="line-number">05</span>
                    <span class="line-content">    { <span class="hljs-string">"ctx_size"</span>, <span class="hljs-number">2048</span> },</span>
                 </div><div class="code-line">
                    <span class="line-number">06</span>
                    <span class="line-content">    <span class="hljs-comment">// Add other settings as needed</span></span>
                 </div><div class="code-line">
                    <span class="line-number">07</span>
                    <span class="line-content">};</span>
                 </div><div class="code-line">
                    <span class="line-number">08</span>
                    <span class="line-content"></span>
                 </div><div class="code-line">
                    <span class="line-number">09</span>
                    <span class="line-content">client.UpdateSettings(settings);</span>
                 </div></code></pre>
                </div><p></p>
<br>
<br>
<p></p><h3 style="font-size: 1.6em; font-weight: bold;">Generating Text</h3><p></p>
<br>
<p>To generate text based on a prompt, use the <b>Generate</b> method:</p>
<br>
<br>
<p>                </p><div class="code-block">
                    <div class="code-header">
                        <span class="language">csharp</span>
                        <button onclick="mr.copyCode(this)" class="copy-button">Copy</button>
                    </div>
                    <pre class="code-content"><code class="hljs language-csharp"><div class="code-line">
                    <span class="line-number">01</span>
                    <span class="line-content"><span class="hljs-built_in">string</span> prompt = <span class="hljs-string">"Once upon a time..."</span>;</span>
                 </div><div class="code-line">
                    <span class="line-number">02</span>
                    <span class="line-content"><span class="hljs-built_in">string</span> generatedText = <span class="hljs-keyword">await</span> client.Generate(prompt);</span>
                 </div><div class="code-line">
                    <span class="line-number">03</span>
                    <span class="line-content">Console.WriteLine(generatedText);</span>
                 </div></code></pre>
                </div><p></p>
<br>
<br>
<p>You can customize the generation parameters:</p>
<br>
<br>
<p>                </p><div class="code-block">
                    <div class="code-header">
                        <span class="language">csharp</span>
                        <button onclick="mr.copyCode(this)" class="copy-button">Copy</button>
                    </div>
                    <pre class="code-content"><code class="hljs language-csharp"><div class="code-line">
                    <span class="line-number">01</span>
                    <span class="line-content"><span class="hljs-built_in">string</span> generatedText = <span class="hljs-keyword">await</span> client.Generate(</span>
                 </div><div class="code-line">
                    <span class="line-number">02</span>
                    <span class="line-content">    prompt,</span>
                 </div><div class="code-line">
                    <span class="line-number">03</span>
                    <span class="line-content">    nPredict: <span class="hljs-number">100</span>,</span>
                 </div><div class="code-line">
                    <span class="line-number">04</span>
                    <span class="line-content">    temperature: <span class="hljs-number">0.7</span>,</span>
                 </div><div class="code-line">
                    <span class="line-number">05</span>
                    <span class="line-content">    topK: <span class="hljs-number">40</span>,</span>
                 </div><div class="code-line">
                    <span class="line-number">06</span>
                    <span class="line-content">    topP: <span class="hljs-number">0.9</span></span>
                 </div><div class="code-line">
                    <span class="line-number">07</span>
                    <span class="line-content">);</span>
                 </div></code></pre>
                </div><p></p>
<br>
<br>
<p></p><h3 style="font-size: 1.6em; font-weight: bold;">Tokenizing and Detokenizing</h3><p></p>
<br>
<p>To tokenize a prompt, use the <b>TokenizeAsync</b> method:</p>
<br>
<br>
<p>                </p><div class="code-block">
                    <div class="code-header">
                        <span class="language">csharp</span>
                        <button onclick="mr.copyCode(this)" class="copy-button">Copy</button>
                    </div>
                    <pre class="code-content"><code class="hljs language-csharp"><div class="code-line">
                    <span class="line-number">01</span>
                    <span class="line-content">List&lt;<span class="hljs-built_in">string</span>&gt; tokens = <span class="hljs-keyword">await</span> client.TokenizeAsync(<span class="hljs-string">"Hello, how are you?"</span>);</span>
                 </div></code></pre>
                </div><p></p>
<br>
<br>
<p>To detokenize a list of tokens back into text, use the <b>DetokenizeAsync</b> method:</p>
<br>
<br>
<p>                </p><div class="code-block">
                    <div class="code-header">
                        <span class="language">csharp</span>
                        <button onclick="mr.copyCode(this)" class="copy-button">Copy</button>
                    </div>
                    <pre class="code-content"><code class="hljs language-csharp"><div class="code-line">
                    <span class="line-number">01</span>
                    <span class="line-content"><span class="hljs-built_in">string</span> detokenizedText = <span class="hljs-keyword">await</span> client.DetokenizeAsync(tokens);</span>
                 </div><div class="code-line">
                    <span class="line-number">02</span>
                    <span class="line-content">Console.WriteLine(detokenizedText);</span>
                 </div></code></pre>
                </div><p></p>
<br>
<br>
<p></p><h3 style="font-size: 1.6em; font-weight: bold;">Managing Messages</h3><p></p>
<br>
<p>You can create formatted messages using the templates loaded earlier:</p>
<br>
<br>
<p>                </p><div class="code-block">
                    <div class="code-header">
                        <span class="language">csharp</span>
                        <button onclick="mr.copyCode(this)" class="copy-button">Copy</button>
                    </div>
                    <pre class="code-content"><code class="hljs language-csharp"><div class="code-line">
                    <span class="line-number">01</span>
                    <span class="line-content"><span class="hljs-built_in">string</span> userMessage = client.UserMessage(<span class="hljs-string">"John"</span>);</span>
                 </div><div class="code-line">
                    <span class="line-number">02</span>
                    <span class="line-content"><span class="hljs-built_in">string</span> aiMessage = client.AiMessage(<span class="hljs-string">"Assistant"</span>);</span>
                 </div><div class="code-line">
                    <span class="line-number">03</span>
                    <span class="line-content"><span class="hljs-built_in">string</span> systemMessage = client.SystemMessage();</span>
                 </div></code></pre>
                </div><p></p>
<br>
<br>
<p></p><h3 style="font-size: 1.6em; font-weight: bold;">Listing Models and Personalities</h3><p></p>
<br>
<p>To list available models and personalities on the server, use the following methods:</p>
<br>
<br>
<p>                </p><div class="code-block">
                    <div class="code-header">
                        <span class="language">csharp</span>
                        <button onclick="mr.copyCode(this)" class="copy-button">Copy</button>
                    </div>
                    <pre class="code-content"><code class="hljs language-csharp"><div class="code-line">
                    <span class="line-number">01</span>
                    <span class="line-content">List&lt;<span class="hljs-built_in">string</span>&gt; models = <span class="hljs-keyword">await</span> client.ListModels();</span>
                 </div><div class="code-line">
                    <span class="line-number">02</span>
                    <span class="line-content">List&lt;<span class="hljs-built_in">string</span>&gt; personalities = <span class="hljs-keyword">await</span> client.ListMountedPersonalities();</span>
                 </div></code></pre>
                </div><p></p>
<br>
<br>
<p></p><h3 style="font-size: 1.6em; font-weight: bold;">Encoding Images</h3><p></p>
<br>
<p>If you need to encode an image to a base64 string, use the <b>EncodeImage</b> method:</p>
<br>
<br>
<p>                </p><div class="code-block">
                    <div class="code-header">
                        <span class="language">csharp</span>
                        <button onclick="mr.copyCode(this)" class="copy-button">Copy</button>
                    </div>
                    <pre class="code-content"><code class="hljs language-csharp"><div class="code-line">
                    <span class="line-number">01</span>
                    <span class="line-content"><span class="hljs-built_in">string</span> base64Image = client.EncodeImage(<span class="hljs-string">"path/to/image.jpg"</span>, maxImageWidth: <span class="hljs-number">800</span>);</span>
                 </div></code></pre>
                </div><p></p>
<br>
<br>
<p></p><h2 style="font-size: 1.9em; font-weight: bold;">Error Handling</h2><p></p>
<br>
<p>The library includes basic error handling. If an error occurs during an API call, an exception will be caught, and an error message will be printed to the console. You can further enhance error handling by implementing your own logging mechanism.</p>
<br>
<p></p><h2 style="font-size: 1.9em; font-weight: bold;">Conclusion</h2><p></p>
<br>
<p>The <b>Lollms_Client_CS</b> library provides a comprehensive interface for interacting with the LOLLMS API. By following this guide, you should be able to set up the client, generate text, manage templates, and handle various settings effectively. For further customization and advanced usage, refer to the source code and explore the available methods.</p>