using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Segment.Model;
using Segment.Request;

namespace Segment
{
	/// <summary>
	/// A Segment.io .NET client
	/// </summary>
	public class Client : IClient
	{
		#region Fields

		private IRequestHandler _requestHandler;
		private string _writeKey;
		private Config _config;

		#endregion //Fields

		#region Properties

		public string WriteKey
		{
			get
			{
				return _writeKey;
			}
		}

		public Config Config
		{
			get
			{
				return _config;
			}
		}

		public LogDelegate Logger { get; set; }

		#endregion //Properties

		#region Initialization

		public Client()
		{
		}

		/// <summary>
		/// Creates a new REST client with a specified API writeKey and default config
		/// </summary>
		/// <param name="writeKey"></param>
		public Client(string writeKey) : this(writeKey, new Config())
		{
		}

		/// <summary>
		/// Creates a new REST client with a specified API writeKey and default config
		/// </summary>
		/// <param name="writeKey"></param>
		/// <param name="config"></param>
		public Client(string writeKey, Config config)
		{
			Initialize(writeKey, config);
		}

		/// <summary>
		/// Creates a new REST client with a specified API writeKey and default config
		/// </summary>
		/// <param name="writeKey"></param>
		public void Initialize(string writeKey)
		{
			Initialize(writeKey, new Config());
		}

		/// <summary>
		/// Creates a new REST client with a specified API writeKey and default config
		/// </summary>
		/// <param name="writeKey"></param>
		/// <param name="config"></param>
		public virtual void Initialize(string writeKey, Config config)
		{
			if (String.IsNullOrEmpty(writeKey))
			{
				throw new InvalidOperationException("Please supply a valid writeKey to initialize.");
			}

			this._writeKey = writeKey;
			this._config = config ?? new Config();

			_requestHandler = new RequestHandler(_writeKey, Config.Host, Config.Timeout);
		}

		#endregion //Initialization

		#region Public Methods

		#region Identify

		/// <summary>
		/// Identifying a visitor ties all of their actions to an ID you
		/// recognize and records visitor traits you can segment by.
		/// </summary>
		///
		/// <param name="userId">The visitor's identifier after they log in, or you know
		/// who they are. By
		/// explicitly identifying a user, you tie all of their actions to their identity.</param>
		///
		/// <param name="traits">A dictionary with keys like "email", "name", “subscriptionPlan” or
		/// "friendCount”. You can segment your users by any trait you record.
		/// Pass in values in key-value format. String key, then its value
		/// { String, Integer, Boolean, Double, or Date are acceptable types for a value. } </param>
		///
		public Task<bool> Identify (string userId, IDictionary<string, object> traits)
		{
			return Identify(userId, traits, null);
		}

		/// <summary>
		/// Identifying a visitor ties all of their actions to an ID you
		/// recognize and records visitor traits you can segment by.
		/// </summary>
		///
		/// <param name="userId">The visitor's identifier after they log in, or you know
		/// who they are. By
		/// explicitly identifying a user, you tie all of their actions to their identity.</param>
		///
		/// <param name="traits">A dictionary with keys like "email", "name", “subscriptionPlan” or
		/// "friendCount”. You can segment your users by any trait you record.
		/// Pass in values in key-value format. String key, then its value
		/// { String, Integer, Boolean, Double, or Date are acceptable types for a value. } </param>
		///
		/// <param name="options">Options allowing you to set timestamp, anonymousId, target integrations,
		/// and the context of th emessage.</param>
		///
		public virtual Task<bool> Identify (string userId, IDictionary<string, object> traits, Options options)
		{
			EnsureId(userId, options);
			return Enqueue(new Identify(userId, traits, options));
		}

		#endregion //Identify

		#region Group

		/// <summary>
		/// The `group` method lets you associate a user with a group. Be it a company, 
		/// organization, account, project, team or whatever other crazy name you came up 
		/// with for the same concept! It also lets you record custom traits about the 
		/// group, like industry or number of employees.
		/// </summary>
		///
		/// <param name="userId">The visitor's database identifier after they log in, or you know
		/// who they are. By explicitly grouping a user, you tie all of their actions to their group.</param>
		///
		/// <param name="groupId">The group's database identifier after they log in, or you know
		/// who they are.</param>
		///
		/// <param name="options">Options allowing you to set timestamp, anonymousId, target integrations,
		/// and the context of th emessage.</param>
		///
		public Task<bool> Group (string userId, string groupId, Options options)
		{
			return Group(userId, groupId, null, options);
		}

		/// <summary>
		/// The `group` method lets you associate a user with a group. Be it a company, 
		/// organization, account, project, team or whatever other crazy name you came up 
		/// with for the same concept! It also lets you record custom traits about the 
		/// group, like industry or number of employees.
		/// </summary>
		///
		/// <param name="userId">The visitor's database identifier after they log in, or you know
		/// who they are. By explicitly grouping a user, you tie all of their actions to their group.</param>
		///
		/// <param name="groupId">The group's database identifier after they log in, or you know
		/// who they are.</param>
		///
		/// <param name="traits">A dictionary with group keys like "name", “subscriptionPlan”. 
		/// You can segment your users by any trait you record. Pass in values in key-value format. 
		/// String key, then its value { String, Integer, Boolean, Double, or Date are acceptable types for a value. } </param>
		///
		public Task<bool> Group (string userId, string groupId, IDictionary<string, object> traits)
		{
			return Group(userId, groupId, traits, null);
		}

		/// <summary>
		/// The `group` method lets you associate a user with a group. Be it a company, 
		/// organization, account, project, team or whatever other crazy name you came up 
		/// with for the same concept! It also lets you record custom traits about the 
		/// group, like industry or number of employees.
		/// </summary>
		///
		/// <param name="userId">The visitor's database identifier after they log in, or you know
		/// who they are. By explicitly grouping a user, you tie all of their actions to their group.</param>
		///
		/// <param name="groupId">The group's database identifier after they log in, or you know
		/// who they are.</param>
		///
		/// <param name="traits">A dictionary with group keys like "name", “subscriptionPlan”. 
		/// You can segment your users by any trait you record. Pass in values in key-value format. 
		/// String key, then its value { String, Integer, Boolean, Double, or Date are acceptable types for a value. } </param>
		///
		/// <param name="options">Options allowing you to set timestamp, anonymousId, target integrations,
		/// and the context of th emessage.</param>
		///
		public Task<bool> Group (string userId, string groupId, IDictionary<string, object> traits, Options options)
		{
			EnsureId(userId, options);

			if (String.IsNullOrEmpty(groupId))
				throw new InvalidOperationException("Please supply a valid groupId to call #Group.");

			return Enqueue(new Group(userId, groupId, traits, options));
		}

		#endregion //Group

		#region Track

		/// <summary>
		/// Whenever a user triggers an event on your site, you’ll want to track it.
		/// </summary>
		///
		/// <param name="userId">The visitor's identifier after they log in, or you know
		/// who they are. </param>
		///
		/// <param name="eventName">The event name you are tracking. It is recommended
		/// that it is in human readable form. For example, "Bought T-Shirt"
		/// or "Started an exercise"</param>
		///
		public Task<bool> Track(string userId, string eventName)
		{
			return Track(userId, eventName, null, null);
		}

		/// <summary>
		/// Whenever a user triggers an event on your site, you’ll want to track it.
		/// </summary>
		///
		/// <param name="userId">The visitor's identifier after they log in, or you know
		/// who they are. </param>
		///
		/// <param name="eventName">The event name you are tracking. It is recommended
		/// that it is in human readable form. For example, "Bought T-Shirt"
		/// or "Started an exercise"</param>
		///
		/// <param name="properties"> A dictionary with items that describe the event
		/// in more detail. This argument is optional, but highly recommended —
		/// you’ll find these properties extremely useful later.</param>
		///
		public Task<bool> Track(string userId, string eventName, IDictionary<string, object> properties)
		{
			return Track(userId, eventName, properties, null);
		}

		/// <summary>
		/// Whenever a user triggers an event on your site, you’ll want to track it
		/// so that you can analyze and segment by those events later.
		/// </summary>
		///
		/// <param name="userId">The visitor's identifier after they log in, or you know
		/// who they are. By
		/// explicitly identifying a user, you tie all of their actions to their identity.
		/// This makes it possible for you to run things like segment-based email campaigns.</param>
		///
		/// <param name="eventName">The event name you are tracking. It is recommended
		/// that it is in human readable form. For example, "Bought T-Shirt"
		/// or "Started an exercise"</param>
		///
		/// <param name="options">Options allowing you to set timestamp, anonymousId, target integrations,
		/// and the context of th emessage.</param>
		/// 
		///
		public Task<bool> Track(string userId, string eventName, Options options)
		{
			return Track(userId, eventName, null, options);
		}

		/// <summary>
		/// Whenever a user triggers an event on your site, you’ll want to track it
		/// so that you can analyze and segment by those events later.
		/// </summary>
		///
		/// <param name="userId">The visitor's identifier after they log in, or you know
		/// who they are. By
		/// explicitly identifying a user, you tie all of their actions to their identity.
		/// This makes it possible for you to run things like segment-based email campaigns.</param>
		///
		/// <param name="eventName">The event name you are tracking. It is recommended
		/// that it is in human readable form. For example, "Bought T-Shirt"
		/// or "Started an exercise"</param>
		///
		/// <param name="properties"> A dictionary with items that describe the event
		/// in more detail. This argument is optional, but highly recommended —
		/// you’ll find these properties extremely useful later.</param>
		///
		/// <param name="options">Options allowing you to set timestamp, anonymousId, target integrations,
		/// and the context of th emessage.</param>
		/// 
		///
		public Task<bool> Track(string userId, string eventName, IDictionary<string, object> properties, Options options)
		{
			EnsureId(userId, options);

			if (String.IsNullOrEmpty(eventName))
				throw new InvalidOperationException("Please supply a valid event to Track.");

			return Enqueue(new Track(userId, eventName, properties, options));
		}

		#endregion //Track

		#region Alias

		/// <summary>
		/// Aliases an anonymous user into an identified user.
		/// </summary>
		/// 
		/// <param name="previousId">The anonymous user's id before they are logged in.</param>
		/// 
		/// <param name="userId">the identified user's id after they're logged in.</param>
		/// 
		public Task<bool> Alias (string previousId, string userId)
		{
			return Alias(previousId, userId, null);
		}

		/// <summary>
		/// Aliases an anonymous user into an identified user.
		/// </summary>
		/// 
		/// <param name="previousId">The anonymous user's id before they are logged in.</param>
		/// 
		/// <param name="userId">the identified user's id after they're logged in.</param>
		///
		/// <param name="options">Options allowing you to set timestamp, anonymousId, target integrations,
		/// and the context of th emessage.</param>
		/// 
		public Task<bool> Alias (string previousId, string userId, Options options)
		{
			if (String.IsNullOrEmpty(previousId))
				throw new InvalidOperationException("Please supply a valid 'previousId' to Alias.");

			if (String.IsNullOrEmpty(userId))
				throw new InvalidOperationException("Please supply a valid 'to' to Alias.");

			return Enqueue(new Alias(previousId, userId, options));
		}

		#endregion //Alias

		#region Page

		/// <summary>
		/// The `page` method let your record whenever a user sees a webpage on 
		/// your website, and attach a `name`, `category` or `properties` to the webpage load. 
		/// </summary>
		///
		/// <param name="userId">The visitor's identifier after they log in, or you know
		/// who they are. By explicitly identifying a user, you tie all of their actions to their identity.
		/// This makes it possible for you to run things like segment-based email campaigns.</param>
		///
		/// <param name="name">The name of the webpage, like "Signup", "Login"</param>
		///
		public Task<bool> Page (string userId, string name)
		{
			return Page(userId, name, null, null, null);
		}

		/// <summary>
		/// The `page` method let your record whenever a user sees a webpage on 
		/// your website, and attach a `name`, `category` or `properties` to the webpage load. 
		/// </summary>
		///
		/// <param name="userId">The visitor's identifier after they log in, or you know
		/// who they are. By explicitly identifying a user, you tie all of their actions to their identity.
		/// This makes it possible for you to run things like segment-based email campaigns.</param>
		///
		/// <param name="name">The name of the webpage, like "Signup", "Login"</param>
		///
		/// <param name="options">Options allowing you to set timestamp, anonymousId, target integrations,
		/// and the context of th emessage.</param>
		///
		public Task<bool> Page (string userId, string name, Options options)
		{
			return Page(userId, name, null, null, options);
		}

		/// <summary>
		/// The `page` method let your record whenever a user sees a webpage on 
		/// your website, and attach a `name`, `category` or `properties` to the webpage load. 
		/// </summary>
		///
		/// <param name="userId">The visitor's identifier after they log in, or you know
		/// who they are. By explicitly identifying a user, you tie all of their actions to their identity.
		/// This makes it possible for you to run things like segment-based email campaigns.</param>
		///
		/// <param name="name">The name of the webpage, like "Signup", "Login"</param>
		/// 
		/// <param name="category">The (optional) category of the webpage, like "Authentication", "Sports"</param>
		///
		public Task<bool> Page (string userId, string name, string category)
		{
			return Page(userId, name, category, null, null);
		}

		/// <summary>
		/// The `page` method let your record whenever a user sees a webpage on 
		/// your website, and attach a `name`, `category` or `properties` to the webpage load. 
		/// </summary>
		///
		/// <param name="userId">The visitor's identifier after they log in, or you know
		/// who they are. By explicitly identifying a user, you tie all of their actions to their identity.
		/// This makes it possible for you to run things like segment-based email campaigns.</param>
		///
		/// <param name="name">The name of the webpage, like "Signup", "Login"</param>
		///
		/// <param name="properties"> A dictionary with items that describe the page
		/// in more detail. This argument is optional, but highly recommended —
		/// you’ll find these properties extremely useful later.</param>
		///
		public Task<bool> Page (string userId, string name, IDictionary<string, object> properties)
		{
			return Page(userId, name, null, properties, null);
		}

		/// <summary>
		/// The `page` method let your record whenever a user sees a webpage on 
		/// your website, and attach a `name`, `category` or `properties` to the webpage load. 
		/// </summary>
		///
		/// <param name="userId">The visitor's identifier after they log in, or you know
		/// who they are. By explicitly identifying a user, you tie all of their actions to their identity.
		/// This makes it possible for you to run things like segment-based email campaigns.</param>
		///
		/// <param name="name">The name of the webpage, like "Signup", "Login"</param>
		///
		/// <param name="properties"> A dictionary with items that describe the page
		/// in more detail. This argument is optional, but highly recommended —
		/// you’ll find these properties extremely useful later.</param>
		///
		/// <param name="options">Options allowing you to set timestamp, anonymousId, target integrations,
		/// and the context of th emessage.</param>
		///
		public Task<bool> Page (string userId, string name, IDictionary<string, object> properties, Options options)
		{
			return Page(userId, name, null, properties, options);
		}

		/// <summary>
		/// The `page` method let your record whenever a user sees a webpage on 
		/// your website, and attach a `name`, `category` or `properties` to the webpage load. 
		/// </summary>
		///
		/// <param name="userId">The visitor's identifier after they log in, or you know
		/// who they are. By explicitly identifying a user, you tie all of their actions to their identity.
		/// This makes it possible for you to run things like segment-based email campaigns.</param>
		///
		/// <param name="name">The name of the webpage, like "Signup", "Login"</param>
		/// 
		/// <param name="category">The (optional) category of the mobile screen, like "Authentication", "Sports"</param>
		///
		/// <param name="properties"> A dictionary with items that describe the page
		/// in more detail. This argument is optional, but highly recommended —
		/// you’ll find these properties extremely useful later.</param>
		///
		/// <param name="options">Options allowing you to set timestamp, anonymousId, target integrations,
		/// and the context of th emessage.</param>
		///
		public Task<bool> Page (string userId, string name, string category, IDictionary<string, object> properties, Options options)
		{
			EnsureId(userId, options);

			if (String.IsNullOrEmpty(name))
				throw new InvalidOperationException("Please supply a valid name to #Page.");

			return Enqueue(new Page(userId, name, category, properties, options));
		}

		#endregion //Page

		#region Screen

		/// <summary>
		/// The `screen` method let your record whenever a user sees a mobile screen on 
		/// your mobile app, and attach a `name`, `category` or `properties` to the screen. 
		/// </summary>
		///
		/// <param name="userId">The visitor's identifier after they log in, or you know
		/// who they are. By
		/// explicitly identifying a user, you tie all of their actions to their identity.
		/// This makes it possible for you to run things like segment-based email campaigns.</param>
		///
		/// <param name="name">The name of the mobile screen, like "Signup", "Login"</param>
		///
		public Task<bool> Screen (string userId, string name)
		{
			return Screen(userId, name, null, null, null);
		}

		/// <summary>
		/// The `screen` method let your record whenever a user sees a mobile screen on 
		/// your mobile app, and attach a `name`, `category` or `properties` to the screen. 
		/// </summary>
		///
		/// <param name="userId">The visitor's identifier after they log in, or you know
		/// who they are. By
		/// explicitly identifying a user, you tie all of their actions to their identity.
		/// This makes it possible for you to run things like segment-based email campaigns.</param>
		///
		/// <param name="name">The name of the mobile screen, like "Signup", "Login"</param>
		///
		/// <param name="options">Options allowing you to set timestamp, anonymousId, target integrations,
		/// and the context of th emessage.</param>
		///
		public Task<bool> Screen (string userId, string name, Options options)
		{
			return Screen(userId, name, null, null, options);
		}

		/// <summary>
		/// The `screen` method let your record whenever a user sees a mobile screen on 
		/// your mobile app, and attach a `name`, `category` or `properties` to the screen. 
		/// </summary>
		///
		/// <param name="userId">The visitor's identifier after they log in, or you know
		/// who they are. By
		/// explicitly identifying a user, you tie all of their actions to their identity.
		/// This makes it possible for you to run things like segment-based email campaigns.</param>
		///
		/// <param name="name">The name of the mobile screen, like "Signup", "Login"</param>
		/// 
		/// <param name="category">The (optional) category of the mobile screen, like "Authentication", "Sports"</param>
		///
		public Task<bool> Screen (string userId, string name, string category)
		{
			return Screen(userId, name, category, null, null);
		}

		/// <summary>
		/// The `screen` method let your record whenever a user sees a mobile screen on 
		/// your mobile app, and attach a `name`, `category` or `properties` to the screen. 
		/// </summary>
		///
		/// <param name="userId">The visitor's identifier after they log in, or you know
		/// who they are. By
		/// explicitly identifying a user, you tie all of their actions to their identity.
		/// This makes it possible for you to run things like segment-based email campaigns.</param>
		///
		/// <param name="name">The name of the mobile screen, like "Signup", "Login"</param>
		///
		/// <param name="properties"> A dictionary with items that describe the screen
		/// in more detail. This argument is optional, but highly recommended —
		/// you’ll find these properties extremely useful later.</param>
		///
		public Task<bool> Screen (string userId, string name, IDictionary<string, object> properties)
		{
			return Screen(userId, name, null, properties, null);
		}

		/// <summary>
		/// The `screen` method let your record whenever a user sees a mobile screen on 
		/// your mobile app, and attach a `name`, `category` or `properties` to the screen. 
		/// </summary>
		///
		/// <param name="userId">The visitor's identifier after they log in, or you know
		/// who they are. By
		/// explicitly identifying a user, you tie all of their actions to their identity.
		/// This makes it possible for you to run things like segment-based email campaigns.</param>
		///
		/// <param name="name">The name of the mobile screen, like "Signup", "Login"</param>
		///
		/// <param name="properties"> A dictionary with items that describe the screen
		/// in more detail. This argument is optional, but highly recommended —
		/// you’ll find these properties extremely useful later.</param>
		///
		/// <param name="options">Options allowing you to set timestamp, anonymousId, target integrations,
		/// and the context of th emessage.</param>
		///
		public Task<bool> Screen (string userId, string name, IDictionary<string, object> properties, Options options)
		{
			return Screen(userId, name, null, properties, options);
		}

		/// <summary>
		/// The `screen` method let your record whenever a user sees a mobile screen on 
		/// your mobile app, and attach a `name`, `category` or `properties` to the screen. 
		/// </summary>
		///
		/// <param name="userId">The visitor's identifier after they log in, or you know
		/// who they are. By
		/// explicitly identifying a user, you tie all of their actions to their identity.
		/// This makes it possible for you to run things like segment-based email campaigns.</param>
		///
		/// <param name="name">The name of the mobile screen, like "Signup", "Login"</param>
		/// 
		/// <param name="category">The (optional) category of the mobile screen, like "Authentication", "Sports"</param>
		///
		/// <param name="properties"> A dictionary with items that describe the screen
		/// in more detail. This argument is optional, but highly recommended —
		/// you’ll find these properties extremely useful later.</param>
		///
		/// <param name="options">Options allowing you to set timestamp, anonymousId, target integrations,
		/// and the context of th emessage.</param>
		///
		public Task<bool> Screen (string userId, string name, string category, IDictionary<string, object> properties, Options options)
		{
			EnsureId(userId, options);

			if (String.IsNullOrEmpty(name))
				throw new InvalidOperationException("Please supply a valid name to #Screen.");

			return Enqueue(new Screen(userId, name, category, properties, options));
		}

		#endregion //Screen

		#endregion //Public Methods

		#region Private Methods

		private async Task<bool> Enqueue(BaseAction action)
		{
			return await _requestHandler.Process(action, Logger);
		}

		protected void EnsureId(String userId, Options options)
		{
			if (String.IsNullOrEmpty (userId) && String.IsNullOrEmpty (options?.AnonymousId)) {
				throw new InvalidOperationException ("Please supply a valid id (either userId or anonymousId.");
			}
		}

		#endregion //Private Methods
	}
}
