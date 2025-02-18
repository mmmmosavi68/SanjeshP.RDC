const { element } = require("prop-types");
const { Children } = require("react");

const menu = [
    {
        pageCode: 100,
        showMenu:1,
        title: "خانه",
        path: "/",
        Api_URL: "/",
        icon: <AiIcons.Home />,
        cName: "nav-text",
    },
    {
        pageCode: 101,
        showMenu:1,
        title: "مدیریت کاربران",
        path: "#",
        Api_URL: "/",
        icon: <AiIcons.Modiriat />,
        cName: "nav-text",
        Children: [
            {
                pageCode: 1011,
                showMenu:1,
                title: "فهرست کاربران",
                path: "users",
                Api_URL: "/",
                icon: <AiIcons.user />,
                cName: "nav-text",
                Children: [
                    {
                        pageCode: 10111,
                        showMenu:0,
                        title: "جدول فهرست کاربران غربال شده ",
                        path: "users",
                        Api_URL: "/api/v1/user/sieve",
                        icon: <AiIcons.sieve />,
                        cName: "nav-text",
                        Children: [
                            {
                                pageCode: 101111,
                                showMenu:0,
                                title: "ثبت کاربر جدید",
                                path: "/users",
                                Api_URL: "/api/v1/user/Create",
                                icon: <AiIcons.Create />,
                                cName: "nav-text",
                            },
                            {
                                pageCode: 101112,
                                showMenu:0,
                                title: "جزئیات کاربر",
                                path: "/users/:id",
                                Api_URL: "/",
                                icon: <AiIcons.GetById />,
                                cName: "nav-text",
                                Children: [
                                    pageCode: 1011121,
                                    showMenu:0,
                                    title: "نمایش جزئیات کاربر",
                                    path: "/users/:id",
                                    Api_URL: "/api/v1/User/GetById",
                                    icon: <AiIcons.GetById />,
                                    cName: "nav-text",
                                ]
                            },
                            {
                                pageCode: 101113,
                                showMenu:0,
                                title: "ویرایش کاربر",
                                path: "/users",
                                Api_URL: "/api/v1/user/Update",
                                icon: <AiIcons.Update />,
                                cName: "nav-text",
                            },
                            {
                                pageCode: 101114,
                                showMenu:0,
                                title: "حذف کاربر",
                                path: "/users",
                                Api_URL: "/api/v1/user/Delete",
                                icon: <AiIcons.Delete />,
                                cName: "nav-text",
                            },
                            {
                                pageCode: 101115,
                                showMenu:0,
                                title: "نمایش درخت دسترسی کاربر",
                                path: "/users",
                                Api_URL: "/api/v1/user/MenuBar_WithUserSelectedAccess",
                                icon: <AiIcons.accessMenu />,
                                cName: "nav-text",
                                Children: [
                                    {
                                        pageCode: 1011151,
                                        showMenu:0,
                                        title: "افزودن / ویرایش / حذف دسترسی کاربر",
                                        path: "/users",
                                        Api_URL: "/api/v1/accessMenu/Modify_SelectedNodes_SelectedUser",
                                        icon: <AiIcons.SieveGroupUsers />,
                                        cName: "nav-text",
                                    },
                                ],
                            },
                        ]
                    },
                ],
            },
            {
                pageCode: 1012,
                showMenu:1,
                itle: "فهرست گروه‌ها",
                path: "/groups",
                Api_URL: "/",
                cName: "nav-text",
                Children: [
                    {
                        pageCode: 10121,
                        showMenu:0,
                        title: "جدول فهرست گروه‌های غربال شده",
                        path: "/groups",
                        Api_URL: "/api/v1/group/sieve",
                        icon: <AiIcons.sieve />,
                        cName: "nav-text",
                        Children: [
                            {
                                pageCode: 101211,
                                showMenu:0,
                                title: "ثبت گروه جدید",
                                path: "/groups",
                                Api_URL: "/api/v1/group/Create",
                                icon: <AiIcons.Create />,
                                cName: "nav-text",
                            },
                            {
                                pageCode: 101212,
                                showMenu:0,
                                title: "ویرایش گروه",
                                path: "/groups",
                                Api_URL: "/api/v1/group/Update",
                                icon: <AiIcons.Update />,
                                cName: "nav-text",
                            },
                            {
                                pageCode: 101213,
                                showMenu:0,
                                title: "حذف گروه",
                                path: "/groups",
                                Api_URL: "/api/v1/group/Delete",
                                icon: <AiIcons.Delete />,
                                cName: "nav-text",
                            },
                            {
                                pageCode: 101214,
                                showMenu:0,
                                title: "نمایش درخت دسترسی گروه",
                                path: "/groups",
                                Api_URL: "/api/v1/group/MenuBar_WithGroupSelectedAccess",
                                icon: <AiIcons.MenuBar_WithGroupSelectedAccess />,
                                cName: "nav-text",
                                Children: [
                                    {
                                        pageCode: 1012141,
                                        showMenu:0,
                                        title: "افزودن / ویرایش / حذف دسترسی گروه کاربری",
                                        path: "/groups",
                                        Api_URL: "/api/v1/accessMenuGroup/Modify_SelectedNodes_SelectedGroup",
                                        icon: <AiIcons.Modify_SelectedNodes_SelectedGroup />,
                                        cName: "nav-text",
                                    },
                                ],
                            },
                            {
                                pageCode: 101215,
                                showMenu:0,
                                title: "نمایش فهرست کاربران گروه",
                                path: "/groups/userlistgroup/:id",
                                Api_URL: "/",
                                icon: <AiIcons.userGroup />,
                                cName: "nav-text",
                                Children: [
                                    {
                                        pageCode: 1012151,
                                        showMenu:0,
                                        title: "جدول فهرست کاربران گروه غربال شده",
                                        path: "/groups/userlistgroup/:id",
                                        Api_URL: "/api/v1/userGroup/SieveGroupUsers",
                                        icon: <AiIcons.SieveGroupUsers />,
                                        cName: "nav-text",
                                        Children: [
                                            {
                                                pageCode: 10121511,
                                                showMenu:0,
                                                title: "حذف کاربر از گروه",
                                                path: "/groups/userlistgroup/:id",
                                                Api_URL: "/api/v1/userGroup/Delete",
                                                icon: <AiIcons.Delete />,
                                                cName: "nav-text",
                                            },
                                            {
                                                pageCode: 10121512,
                                                showMenu:0,
                                                title: "نمایش فهرست کاربران جهت افزودن به گروه",
                                                path: "/groups/userlistgroup/:id",
                                                Api_URL: "/api/v1/userGroup/SieveUsersNotInGroup",
                                                icon: <AiIcons.SieveUsersNotInGroup />,
                                                cName: "nav-text",
                                                Children: [
                                                    {
                                                        pageCode: 101215121,
                                                        showMenu:0,
                                                        title: "غربال کردن فهرست کاربران جهت افزودن به گروه",
                                                        path: "/groups/userlistgroup/:id",
                                                        Api_URL: "/api/v1/userGroup/SieveUsersNotInGroup",
                                                        icon: <AiIcons.SieveUsersNotInGroup />,
                                                        cName: "nav-text",
                                                        Children: [
                                                            {
                                                                pageCode: 1012151211,
                                                                showMenu:0,
                                                                title: "افزودن کاربر به گروه",
                                                                path: "/groups/userlistgroup/:id",
                                                                Api_URL: "/api/v1/userGroup/AddUserGroup",
                                                                icon: <AiIcons.AddUserGroup />,
                                                                cName: "nav-text",
                                                            },
                                                        ]
                                                    },

                                                ]
                                            },
                                        ]
                                    },
                                ],
                            },
                        ]
                    },
                ],
            },
        ]
    },
];
