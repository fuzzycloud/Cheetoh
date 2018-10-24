#!/usr/bin/env bash

set -eu
set -o pipefail

cd "$(dirname "$0")"

# PAKET_BOOTSTRAPPER_EXE=.paket/paket.bootstrapper.exe
PAKET_EXE=.paket/paket.exe
FAKE_EXE=fake

FSIARGS=""
FSIARGS2=""
OS=${OS:-"unknown"}
if [ "$OS" != "Windows_NT" ]
then
  # Can't use FSIARGS="--fsiargs -d:MONO" in zsh, so split it up
  # (Can't use arrays since dash can't handle them)
  FSIARGS="--fsiargs"
  FSIARGS2="-d:MONO"
fi

run() {
  if [ "$OS" != "Windows_NT" ]
  then
    mono "$@"
  else
    "$@"
  fi
}

# run $PAKET_BOOTSTRAPPER_EXE
run $PAKET_EXE restore
# $FAKE_EXE run build.fsx --target "$@" $FSIARGS $FSIARGS2
$FAKE_EXE run build.fsx --target "$@"
