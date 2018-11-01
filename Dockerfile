FROM vbfox/fable-build:stretch AS builder

WORKDIR /build

RUN dotnet tool install fake-cli -g
ENV PATH="${PATH}:/root/.dotnet/tools"

# Package lock files are copied independently and their respective package
# manager are executed after.
#
# This is voluntary as docker will cache images and only re-create them if
# the already-copied files have changed, by doing that as long as no package
# is installed or updated we keep the cached container and don't need to
# re-download.

# Initialize node_modules
COPY package.json yarn.lock ./
RUN yarn install

# Initialize paket packages
COPY paket.dependencies paket.lock ./
COPY .paket .paket
RUN mono .paket/paket.exe restore

# Copy everything else and run the build
COPY . ./
RUN rm -rf deploy
RUN fake run build.fsx --target Bundle



FROM microsoft/dotnet:2.1-aspnetcore-runtime-alpine
# here I am not putting everyting root instead directory called
WORKDIR /app
COPY --from=builder /build/deploy ./
# here I have to map it to server directory
WORKDIR /app/Server
EXPOSE 8085
ENTRYPOINT [ "dotnet", "Server.dll" ]
