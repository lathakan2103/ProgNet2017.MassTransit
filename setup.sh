cd BasicPatterns
mono .paket/paket.bootstrapper.exe
mono .paket/paket.exe restore
cd ../Saga
mono .paket/paket.bootstrapper.exe
mono .paket/paket.exe restore
cd ..