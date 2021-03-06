﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.DAL.App_data;
using SocialNetwork.DAL.Contexts;
using SocialNetwork.DAL.IContexts;
using SocialNetwork.DAL.IRepositories;
using SocialNetwork.DAL.Repositories;
using SocialNetwork.Logic;
using SocialNetwork.Logic.ILogic;
using System;
using SocialNetwork.Hubs;

namespace SocialNetwork
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(15);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddTransient(_ => new Connection(Configuration.GetConnectionString("DefaultConnection")));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUserContext, UserContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserLogic, UserLogic>();
            services.AddScoped<IPostContext, PostContext>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IPostLogic, PostLogic>();
            services.AddScoped<IFriendRequestContext, FriendRequestContext>();
            services.AddScoped<IFriendRequestRepository, FriendRequestRepository>();
            services.AddScoped<IFriendRequestLogic, FriendRequestLogic>();
            services.AddScoped<IProfilePictureContext, ProfilePictureContext>();
            services.AddScoped<IProfilePictureRepository, ProfilePictureRepository>();
            services.AddScoped<IProfilePictureLogic, ProfilePictureLogic>();
            services.AddScoped<ICommentContext, CommentContext>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ICommentLogic, CommentLogic>();
            services.AddScoped<IStatisticLogic, StatisticLogic>();
            services.AddSignalR();
            services.AddHttpContextAccessor();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseExceptionHandler("/Exception/Error");
            app.UseStatusCodePagesWithRedirects("/Exception/Error/{0}");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Login}/{id?}");
            });

            app.UseSignalR(route =>
            {
                route.MapHub<ChatHub>("/ChatHub");
            });
        }
    }
}
