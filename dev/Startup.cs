/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

using Microsoft.EntityFrameworkCore;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Playground;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MagicAppAPI.Context;
using MagicAppAPI.Models;
using MagicAppAPI.GraphQL.Queries;
using MagicAppAPI.GraphQL.Mutations;
using MagicAppAPI.GraphQL;

namespace MagicAppAPI
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
			StaticConfig = configuration;
		}

		public IConfiguration Configuration { get; private set; }
		public static IConfiguration? StaticConfig { get; private set; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{

			services.AddDbContext<MagicAppContext>(opt => opt.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

			services
			.AddGraphQLServer()
			.AddAuthorization()
			.AddQueryType(q => q.Name("Query"))
			.AddType<UserQuery>()
			.AddType<SetQuery>()
			.AddType<CardQuery>()
			.AddType<CollectionQuery>()
			.AddMutationType(m => m.Name("Mutation"))
			.AddType<LoginMutation>()
			.AddType<UserMutation>()
			.AddType<CollectionMutation>()
			//.AddType<UploadType>()
			.AddProjections()
			.AddFiltering()
			.AddSorting();

			//var settings = new Settings();
			//Configuration.Bind(settings);
			//services.AddSingleton(settings);


			services.AddErrorFilter<GraphQLErrorFilter>();

			services.AddCors(options =>
			{
				options.AddDefaultPolicy(builder =>
					builder.SetIsOriginAllowed(_ => true)
					.AllowAnyMethod()
					.AllowAnyHeader()
					.AllowCredentials());
			});


			services.Configure<TokenSettings>(Configuration.GetSection("TokenSettings"));

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidIssuer = Configuration.GetSection("TokenSettings").GetValue<string>("Issuer"),
					ValidateIssuer = true,
					ValidAudience = Configuration.GetSection("TokenSettings").GetValue<string>("Audience"),
					ValidateAudience = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("TokenSettings").GetValue<string>("Key"))),
					ValidateIssuerSigningKey = true
				};
			});
			services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			//services.AddSignalR();

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MagicAppContext context)
		{
			if (env.IsDevelopment())
			{
				//app.UseDeveloperExceptionPage();
			}

			app.UseDefaultFiles();
			app.UseStaticFiles();

			try
			{
				//context.Database.Migrate();
			}
			catch (Exception e)
			{
				Console.WriteLine("Failed to connect to database with exception: " + e);
			}
			//app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());
			app.UseCors();
			app.UseRouting();

			app.UseAuthentication();

			app.UsePlayground(new PlaygroundOptions()
			{
				Path = "/playground",
				QueryPath = "/graphql"
			});

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapGraphQL();
			});
		}
	}
}
