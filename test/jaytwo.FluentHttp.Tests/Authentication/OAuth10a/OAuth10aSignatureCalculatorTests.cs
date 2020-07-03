using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using jaytwo.FluentHttp.Authentication.OAuth10a;
using Xunit;

namespace jaytwo.FluentHttp.Tests.Authentication.OAuth10a
{
    public class OAuth10aSignatureCalculatorTests
    {
        [Fact]
        public void GetSignature_returns_expected_value()
        {
            // arrange
            var oauth = new OAuth10aSignatureCalculator()
            {
                ConsumerKey = "dpf43f3p2l4k3l03",
                ConsumerSecret = "kd94hf93k423kf44",
                Token = "nnch734d00sl2jdk",
                TokenSecret = "pfkkdhi9sl3r4s00",
                Nonce = "kllo9940pd9333jh",
                Timestamp = 1191242096,
                Url = "http://photos.example.net/photos?file=vacation.jpg&size=original",
                HttpMethod = HttpMethod.Get,
                SignatureMethod = "HMAC-SHA1",
                OauthVersion = "1.0",
            };

            var expected = "tR3+Ty81lMeYAr/Fid0kMTYa/WM=";

            // act
            var actual = oauth.GetSignature();

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetSignature_returns_expected_value_etrade()
        {
            // example from: https://developer.etrade.com/getting-started/developer-guides
            //  Note: the documentation is using the displays the new url (api.etrade.com), but is hashed using the old url (etws.etrade.com)...

            // arrange
            var oauth = new OAuth10aSignatureCalculator()
            {
                ConsumerKey = "c5bb4dcb7bd6826c7c4340df3f791188",
                ConsumerSecret = "7d30246211192cda43ede3abd9b393b9",
                Token = "VbiNYl63EejjlKdQM6FeENzcnrLACrZ2JYD6NQROfVI=",
                TokenSecret = "XCF9RzyQr4UEPloA+WlC06BnTfYC1P0Fwr3GUw/B0Es=",
                Nonce = "0bba225a40d1bbac2430aa0c6163ce44",
                Timestamp = 1344885636,
                //Url = "https://api.etrade.com/v1/accounts/list",
                Url = "https://etws.etrade.com/accounts/rest/accountlist",
                HttpMethod = HttpMethod.Get,
                SignatureMethod = "HMAC-SHA1",
                OauthVersion = null,
            };

            var expected = Uri.UnescapeDataString("%2FXiv96DzZabnUG2bzPZIH2RARHM%3D");

            // act
            var actual = oauth.GetSignature();

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetSignature_returns_expected_value_twitter()
        {
            // example from: https://developer.twitter.com/en/docs/basics/authentication/oauth-1-0a/creating-a-signature

            // arrange
            var oauth = new OAuth10aSignatureCalculator()
            {
                ConsumerKey = "xvz1evFS4wEEPTGEFPHBog",
                ConsumerSecret = "kAcSOqF21Fu85e7zjz7ZN2U4ZRhfV3WpwPAoE3Z7kBw",
                Token = "370773112-GmHxMAgYyLbNEtIKZeRNFsMKPR9EyMZeS9weJAEb",
                TokenSecret = "LswwdoUaIvS8ltyTt5jkRh4J50vUPVVHtR2YPi5kE",
                Nonce = "kYjzVBB8Y0ZFabxSWbWovY3uYSQ2pTgmZeNu2VS4cg",
                Timestamp = 1318622958,
                Url = "https://api.twitter.com/1.1/statuses/update.json",
                HttpMethod = HttpMethod.Post,
                SignatureMethod = "HMAC-SHA1",
                OauthVersion = "1.0",
                BodyParameters = new Dictionary<string, string>()
                    {
                        { "status", "Hello Ladies + Gentlemen, a signed OAuth request!" },
                        { "include_entities", "true" },
                    },
            };

            var expected = "hCtSmYh+iHYCEqBWrE7C7hYmtUk=";

            // act
            var actual = oauth.GetSignature();

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetAuthorizationHeaderValue_returns_expected_value()
        {
            // example from: https://developer.etrade.com/getting-started/developer-guides
            //  Note: the documentation is using the displays the new url (api.etrade.com), but is hashed using the old url (etws.etrade.com)...

            // arrange
            var oauth = new OAuth10aSignatureCalculator()
            {
                ConsumerKey = "c5bb4dcb7bd6826c7c4340df3f791188",
                ConsumerSecret = "7d30246211192cda43ede3abd9b393b9",
                Token = "VbiNYl63EejjlKdQM6FeENzcnrLACrZ2JYD6NQROfVI=",
                TokenSecret = "XCF9RzyQr4UEPloA+WlC06BnTfYC1P0Fwr3GUw/B0Es=",
                Nonce = "0bba225a40d1bbac2430aa0c6163ce44",
                Timestamp = 1344885636,
                //Url = "https://api.etrade.com/v1/accounts/list",
                Url = "https://etws.etrade.com/accounts/rest/accountlist",
                HttpMethod = HttpMethod.Get,
                SignatureMethod = "HMAC-SHA1",
                OauthVersion = null,
            };

            var expected = "OAuth oauth_nonce=\"0bba225a40d1bbac2430aa0c6163ce44\",oauth_timestamp=\"1344885636\",oauth_consumer_key=\"c5bb4dcb7bd6826c7c4340df3f791188\",oauth_token=\"VbiNYl63EejjlKdQM6FeENzcnrLACrZ2JYD6NQROfVI%3D\",oauth_signature=\"%2FXiv96DzZabnUG2bzPZIH2RARHM%3D\",oauth_signature_method=\"HMAC-SHA1\"";

            // act
            var actual = oauth.GetAuthorizationHeaderValue();

            // assert
            Assert.Equal(AuthenticationHeaderValue.Parse(expected).Scheme, AuthenticationHeaderValue.Parse(actual).Scheme);
            Assert.Equal(
                AuthenticationHeaderValue.Parse(expected).Parameter.Split(',').OrderBy(x => x),
                AuthenticationHeaderValue.Parse(actual).Parameter.Split(',').OrderBy(x => x));
        }

        [Fact]
        public void GetSignatureBaseStringParameters_returns_expected_value()
        {
            // example from: https://oauth.net/core/1.0a/#rfc.section.A.5.2

            // arrange
            var oauth = new OAuth10aSignatureCalculator()
            {
                ConsumerKey = "dpf43f3p2l4k3l03",
                ConsumerSecret = "kd94hf93k423kf44",
                Token = "nnch734d00sl2jdk",
                TokenSecret = "pfkkdhi9sl3r4s00",
                Nonce = "kllo9940pd9333jh",
                Timestamp = 1191242096,
                Url = "http://photos.example.net/photos?file=vacation.jpg&size=original",
                HttpMethod = HttpMethod.Get,
                SignatureMethod = "HMAC-SHA1",
                OauthVersion = "1.0",
            };

            var expected = new Dictionary<string, string>()
            {
                { "oauth_consumer_key", "dpf43f3p2l4k3l03" },
                { "oauth_token", "nnch734d00sl2jdk" },
                { "oauth_signature_method", "HMAC-SHA1" },
                { "oauth_timestamp", "1191242096" },
                { "oauth_nonce", "kllo9940pd9333jh" },
                { "oauth_version", "1.0" },
                { "file", "vacation.jpg" },
                { "size", "original" },
            };

            // act
            var actual = oauth.GetSignatureBaseStringParameters();

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetSignatureBaseStringParameters_returns_expected_value_twitter()
        {
            // example from: https://developer.twitter.com/en/docs/basics/authentication/oauth-1-0a/creating-a-signature

            // arrange
            var oauth = new OAuth10aSignatureCalculator()
            {
                ConsumerKey = "xvz1evFS4wEEPTGEFPHBog",
                ConsumerSecret = "kAcSOqF21Fu85e7zjz7ZN2U4ZRhfV3WpwPAoE3Z7kBw",
                Token = "370773112-GmHxMAgYyLbNEtIKZeRNFsMKPR9EyMZeS9weJAEb",
                TokenSecret = "LswwdoUaIvS8ltyTt5jkRh4J50vUPVVHtR2YPi5kE",
                Nonce = "kYjzVBB8Y0ZFabxSWbWovY3uYSQ2pTgmZeNu2VS4cg",
                Timestamp = 1318622958,
                Url = "https://api.twitter.com/1.1/statuses/update.json",
                HttpMethod = HttpMethod.Post,
                SignatureMethod = "HMAC-SHA1",
                OauthVersion = "1.0",
                BodyParameters = new Dictionary<string, string>()
                    {
                        { "status", "Hello Ladies + Gentlemen, a signed OAuth request!" },
                        { "include_entities", "true" },
                    },
            };

            var expected = new Dictionary<string, string>()
            {
                { "oauth_consumer_key", "xvz1evFS4wEEPTGEFPHBog" },
                { "oauth_token", "370773112-GmHxMAgYyLbNEtIKZeRNFsMKPR9EyMZeS9weJAEb" },
                { "oauth_signature_method", "HMAC-SHA1" },
                { "oauth_timestamp", "1318622958" },
                { "oauth_nonce", "kYjzVBB8Y0ZFabxSWbWovY3uYSQ2pTgmZeNu2VS4cg" },
                { "oauth_version", "1.0" },
                { "status", "Hello Ladies + Gentlemen, a signed OAuth request!" },
                { "include_entities", "true" },
            };

            // act
            var actual = oauth.GetSignatureBaseStringParameters();

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetSignatureBaseString_returns_expected_value()
        {
            // example from: https://oauth.net/core/1.0a/#sig_base_example

            // arrange
            var parameters = new Dictionary<string, string>()
            {
                { "oauth_consumer_key", "dpf43f3p2l4k3l03" },
                { "oauth_token", "nnch734d00sl2jdk" },
                { "oauth_signature_method", "HMAC-SHA1" },
                { "oauth_timestamp", "1191242096" },
                { "oauth_nonce", "kllo9940pd9333jh" },
                { "oauth_version", "1.0" },
                { "file", "vacation.jpg" },
                { "size", "original" },
            };

            var httpMethod = HttpMethod.Get;
            var url = "http://photos.example.net/photos";

            var expected = "GET&http%3A%2F%2Fphotos.example.net%2Fphotos&file%3Dvacation.jpg%26oauth_consumer_key%3Ddpf43f3p2l4k3l03%26oauth_nonce%3Dkllo9940pd9333jh%26oauth_signature_method%3DHMAC-SHA1%26oauth_timestamp%3D1191242096%26oauth_token%3Dnnch734d00sl2jdk%26oauth_version%3D1.0%26size%3Doriginal";

            // act
            var actual = OAuth10aSignatureCalculator.GetSignatureBaseString(httpMethod, url, parameters);

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetSignatureBaseString_returns_expected_value_etrade()
        {
            // example from: https://developer.etrade.com/getting-started/developer-guides
            //  Note: the documentation is using the displays the new url (api.etrade.com), but is hashed using the old url (etws.etrade.com)...

            // arrange
            var oauth = new OAuth10aSignatureCalculator()
            {
                ConsumerKey = "c5bb4dcb7bd6826c7c4340df3f791188",
                ConsumerSecret = "7d30246211192cda43ede3abd9b393b9",
                Token = "VbiNYl63EejjlKdQM6FeENzcnrLACrZ2JYD6NQROfVI=",
                TokenSecret = "XCF9RzyQr4UEPloA+WlC06BnTfYC1P0Fwr3GUw/B0Es=",
                Nonce = "0bba225a40d1bbac2430aa0c6163ce44",
                Timestamp = 1344885636,
                //Url = "https://api.etrade.com/v1/accounts/list",
                Url = "https://etws.etrade.com/accounts/rest/accountlist",
                HttpMethod = HttpMethod.Get,
                SignatureMethod = "HMAC-SHA1",
                OauthVersion = null,
            };

            var expected = "GET&https%3A%2F%2Fetws.etrade.com%2Faccounts%2Frest%2Faccountlist&oauth_consumer_key%3Dc5bb4dcb7bd6826c7c4340df3f791188%26oauth_nonce%3D0bba225a40d1bbac2430aa0c6163ce44%26oauth_signature_method%3DHMAC-SHA1%26oauth_timestamp%3D1344885636%26oauth_token%3DVbiNYl63EejjlKdQM6FeENzcnrLACrZ2JYD6NQROfVI%253D";

            // act
            var actual = oauth.GetSignatureBaseString();

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetSignatureBaseString_returns_expected_value_twitter()
        {
            // example from: https://developer.twitter.com/en/docs/basics/authentication/oauth-1-0a/creating-a-signature

            // arrange
            var parameters = new Dictionary<string, string>()
            {
                { "oauth_consumer_key", "xvz1evFS4wEEPTGEFPHBog" },
                { "oauth_token", "370773112-GmHxMAgYyLbNEtIKZeRNFsMKPR9EyMZeS9weJAEb" },
                { "oauth_signature_method", "HMAC-SHA1" },
                { "oauth_timestamp", "1318622958" },
                { "oauth_nonce", "kYjzVBB8Y0ZFabxSWbWovY3uYSQ2pTgmZeNu2VS4cg" },
                { "oauth_version", "1.0" },
                { "include_entities", "true" },
                { "status", "Hello Ladies + Gentlemen, a signed OAuth request!" },
            };

            var httpMethod = HttpMethod.Post;
            var url = "https://api.twitter.com/1.1/statuses/update.json";

            var expected = "POST&https%3A%2F%2Fapi.twitter.com%2F1.1%2Fstatuses%2Fupdate.json&include_entities%3Dtrue%26oauth_consumer_key%3Dxvz1evFS4wEEPTGEFPHBog%26oauth_nonce%3DkYjzVBB8Y0ZFabxSWbWovY3uYSQ2pTgmZeNu2VS4cg%26oauth_signature_method%3DHMAC-SHA1%26oauth_timestamp%3D1318622958%26oauth_token%3D370773112-GmHxMAgYyLbNEtIKZeRNFsMKPR9EyMZeS9weJAEb%26oauth_version%3D1.0%26status%3DHello%2520Ladies%2520%252B%2520Gentlemen%252C%2520a%2520signed%2520OAuth%2520request%2521";

            // act
            var actual = OAuth10aSignatureCalculator.GetSignatureBaseString(httpMethod, url, parameters);

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SignBaseString_returns_expected_value()
        {
            // example from: https://oauth.net/core/1.0a/#rfc.section.A.5.2

            // arrange
            var baseString = "GET&http%3A%2F%2Fphotos.example.net%2Fphotos&file%3Dvacation.jpg%26oauth_consumer_key%3Ddpf43f3p2l4k3l03%26oauth_nonce%3Dkllo9940pd9333jh%26oauth_signature_method%3DHMAC-SHA1%26oauth_timestamp%3D1191242096%26oauth_token%3Dnnch734d00sl2jdk%26oauth_version%3D1.0%26size%3Doriginal";
            var consumerKey = "kd94hf93k423kf44";
            var tokenKey = "pfkkdhi9sl3r4s00";

            var expected = "tR3+Ty81lMeYAr/Fid0kMTYa/WM=";

            // act
            var actual = OAuth10aSignatureCalculator.SignBaseString(baseString, consumerKey, tokenKey);

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SignBaseString_returns_expected_value_twitter()
        {
            // example from: https://developer.twitter.com/en/docs/basics/authentication/oauth-1-0a/creating-a-signature

            // arrange
            var baseString = "POST&https%3A%2F%2Fapi.twitter.com%2F1.1%2Fstatuses%2Fupdate.json&include_entities%3Dtrue%26oauth_consumer_key%3Dxvz1evFS4wEEPTGEFPHBog%26oauth_nonce%3DkYjzVBB8Y0ZFabxSWbWovY3uYSQ2pTgmZeNu2VS4cg%26oauth_signature_method%3DHMAC-SHA1%26oauth_timestamp%3D1318622958%26oauth_token%3D370773112-GmHxMAgYyLbNEtIKZeRNFsMKPR9EyMZeS9weJAEb%26oauth_version%3D1.0%26status%3DHello%2520Ladies%2520%252B%2520Gentlemen%252C%2520a%2520signed%2520OAuth%2520request%2521";
            var consumerKey = "kAcSOqF21Fu85e7zjz7ZN2U4ZRhfV3WpwPAoE3Z7kBw";
            var tokenKey = "LswwdoUaIvS8ltyTt5jkRh4J50vUPVVHtR2YPi5kE";

            var expected = "hCtSmYh+iHYCEqBWrE7C7hYmtUk=";

            // act
            var actual = OAuth10aSignatureCalculator.SignBaseString(baseString, consumerKey, tokenKey);

            // assert
            Assert.Equal(expected, actual);
        }
    }
}
