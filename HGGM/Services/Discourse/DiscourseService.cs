using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace HGGM.Services.Discourse
{
    public class DiscourseService
    {
        private readonly Options _options;

        public DiscourseService(IOptions<Options> options)
        {
            _options = options.Value;
        }

        public (string payload, string signature) CreatePayload(
            [NotNull] string nonce,
            [NotNull] string email,
            [NotNull] string externalId,
            [CanBeNull] string username,
            [CanBeNull] string fullName,
            [CanBeNull] string avatarUrl,
            [CanBeNull] string biography,
            [CanBeNull] List<string> addGroups,
            [CanBeNull] List<string> removeGroups,
            bool? admin,
            bool? moderator,
            bool? avatarForceUpdate,
            bool? emailRequireActivation,
            bool? suppressWelcomeMessage)
        {
            var props = new Dictionary<string, string>
            {
                {"nonce", nonce}, {"email", email}, {"external_id", externalId}
            };
            if (username != null) props.Add("username", username);
            if (fullName != null) props.Add("name", fullName);
            if (avatarUrl != null) props.Add("avatar_url", avatarUrl);
            if (avatarForceUpdate == true) props.Add("avatar_force_update", "true");
            if (biography != null) props.Add("bio", biography);
            if (addGroups != null && addGroups.Count > 0) props.Add("&add_groups", string.Join(", ", addGroups));
            if (removeGroups != null && removeGroups.Count > 0)
                props.Add("&remove_groups", string.Join(",", removeGroups));
            if (admin == true) props.Add("admin", "true");
            if (moderator == true) props.Add("moderator", "true");
            if (suppressWelcomeMessage == true) props.Add("suppress_welcome_message", "true");
            if (emailRequireActivation == true) props.Add("&require_activation", "true");

            var payload = Crypto.ConvertStringToBase64(string.Join('&',
                props.Select((k, v) => $"{k}={v}")));
            var signature = Crypto.CreateHmacsha256(_options.Secret, payload);

            return (payload, signature);
        }

        public (string nonce, string returnUrl) OpenPayload(string sso, string sig)
        {
            if (!Crypto.IsSignatureValid(_options.Secret, sso, sig))
                throw new ArgumentException("Signature for this payload is invalid");
            var text = Crypto.ConvertBase64ToString(sso);
            var source = text.Split('&').ToList();
            var nonce = source.Single(x => x.StartsWith("nonce")).Split('=')[1];
            var returnUrl = WebUtility.UrlDecode(source.Single(x => x.StartsWith("return_sso_url")).Split('=')[1]);
            return (nonce, returnUrl);
        }

        public class Options
        {
            public string Secret { get; set; }
        }
    }
}