namespace Helper
open System.Text
open System
open System.Security.Cryptography
open JWT.Builder
open JWT.Algorithms
open System.Collections.Generic
open Shared
open HashidsNet

[<CLIMutable>]
type Payload = {
    UserId : int
    Email : string
    Name : string
    UserType : string
    Organization : string
}


(**
    All password related stuff happend in this module.
    Feel free to change setting and algorith as requied
*)
module Security =
    let createRandomKey() =
        let generator = System.Security.Cryptography.RandomNumberGenerator.Create()
        let randomKey = Array.init 32 byte
        generator.GetBytes(randomKey)
        randomKey

    let utf8Bytes (input: string) = Encoding.UTF8.GetBytes(input)
    let base64 (input: byte[]) = Convert.ToBase64String(input)
    let sha256 = SHA256.Create()
    let sha256Hash (input: byte[]) : byte[] = sha256.ComputeHash(input)


    let verifyPassword password salt hash =
        let salt = Convert.FromBase64String salt
        Array.concat [salt ; utf8Bytes password]
        |> sha256Hash
        |> base64
        |> fun x -> x = hash

    (**
        Generate JWT token with payload. Secret is given in config
        Exp is not given for now.
    *)
    let generateToken (payload : Payload) (conf : IConfig) =
        JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(conf.Security.Secret)
                .AddClaim("user", payload)
                .Build()

    (**
        Get payload value out from token
    *)
    let getTokenValue (token:AuthToken) (conf : IConfig) =
        let (AuthToken t) = token
        let res = JwtBuilder()
                    .WithSecret(conf.Security.Secret)
                    .MustVerifySignature()
                    .Decode<Dictionary<string,Payload>>(t)
        let user = res.["user"]
        user

    (**
        Generate short hash based on user id that can be sent to user for email varification
    *)
    let generateHashIds (userId : int) (conf : IConfig) =
        let hashids = Hashids(conf.Security.Secret, conf.Security.HashLength, conf.Security.Secret)
        hashids.Encode userId

    (**
        Retrive user id from hash id
    *)
    let getIdValue (hashId : string) (conf : IConfig) =
        let hashids = Hashids(conf.Security.Secret, conf.Security.HashLength, conf.Security.Secret)
        hashids.Decode hashId |> Array.head
