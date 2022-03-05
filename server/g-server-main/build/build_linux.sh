export GOPROXY=https://goproxy.io
export GOOS=linux
export GOARCH=amd64

echo "============================="
echo "==== building"
echo "============================="
go build -o g-server

if [[ $? -ne 0 ]]
then
    echo "build failed"
    exit -1
fi

#package when necessary
#echo "============================="
#echo "==== packaging"
#echo "============================="
#tar -czf g-server.tar.gz g-server config
#
#rm -rf dist
#mkdir -p dist
#mv g-server.tar.gz dist/
#
#echo "============================="
#echo "==== clean"
#echo "============================="
#rm -rf "g-server"