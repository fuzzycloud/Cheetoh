namespace global

(**
    Application level config. Can be accessed from server and client both
    Currently getting accessed by server only as in server it is easy to differentiate between dev and prod
*)
type IConfig = {
    Name : string
    AdminUser : AdminUser
    Security : Security
} and AdminUser = {
    Name : string
    Email : string
    Password : string
} and Security = {
    Secret : string
    HashLength : int
    HashChars : string
}


module Config =

    let Dev = {
                    Name = "DemoApp"
                    AdminUser = {
                                    Name = "admin"
                                    Email = "admin@admin.com"
                                    Password = "admin"
                                }
                    Security = {
                                    Secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk"
                                    HashLength = 8
                                    HashChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"
                                }
                }

    let Prod = Dev
    //TODO: Provide Prod setting in line with Dev
