﻿HttpClient client = new();
HttpResponseMessage response = await client.GetAsync("http://www.apple.com/");
WriteLine("Apple home page has {0:N0} bytes.", response.Content.Headers.ContentLength);