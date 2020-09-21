# Configuration Manager Server

This is the server that powers the Palavyr configuration manager. Its an asp.net core console app written in C# that is intended to run on IIS windows server 2019.

# Steps I took to dockerize
1. https://www.youtube.com/watch?v=f0lMGPB10bM&ab_channel=LesJackson
2. https://stackoverflow.com/questions/45270598/how-i-can-dockerize-my-web-api-on-windows
    docker pull microsoft/aspnetcore

3. https://www.red-gate.com/simple-talk/sysadmin/containerization/asp-net-core-with-gitops-dockerizing-an-api-on-aws-ec2/

#### What are those <none> images?
https://www.projectatomic.io/blog/2015/07/what-are-docker-none-none-images/


#### Aother asp net core app with docker tutorial
https://medium.com/@lucaslohrmann/create-a-asp-net-core-web-app-with-docker-5df9e2cc17c7

#### Good Guidance on iis (a reverse proxy) in front of asp.net core (runs on kestrel, but doesn't auto handle ssl)
https://weblog.west-wind.com/posts/2016/jun/06/publishing-and-running-aspnet-core-applications-with-iis#:~:text=It's%20not%20hosted%20inside%20of,this%20self%2Dhosted%20server%20instance.


#### Some good docs on dockerfile for windows:
https://docs.microsoft.com/en-us/virtualization/windowscontainers/manage-docker/manage-windows-dockerfile

#### THE HOLY GRAIL FOR WINDOW IIS DOCKERFILES!
https://github.com/MicrosoftDocs/Virtualization-Documentation/tree/master/windows-container-samples

### Docker hub for server core iis
https://hub.docker.com/_/microsoft-windows-servercore-iis

### How to make a sleep command:

    RUN ping 127.0.0.1 -n 6 > nul



### How to clear the docker cache
https://forums.docker.com/t/how-to-delete-cache/5753

    S) alias docker_clean_images='docker rmi $(docker images -a --filter=dangling=true -q)'

    B) alias docker_clean_ps='docker rm $(docker ps --filter=status=exited --filter=status=created -q)'

If you want to remove ALL of your cache, you first have to make sure all containers are stopped and removed, since you cannot remove an image in use by a container. So something similar

    docker kill $(docker ps -q)
    docker_clean_ps
    docker rmi $(docker images -a -q)`


### What are the steps then?

1. pull windows server core iss, sdk 3.1, asp.net core 3.1
2. Expose port 80 and 443
3. create a working directory call `/src` where we can copy the solution filse
4. create a directory called inetpub/wwwroot where the api build will go
5. build the soln
6. publish the soln't
7. push the image


### How to build your image

Ive successfully built an image that contains asp.net core 3.1. I used the .net sdk image to run the build
commands.

    docker build -t paulegradie-server .

This was actually incorrect I think. It should have been:

    docker build -t paulegradie/paulegradie-server .

#### How to retag the image and push

So I created my image, and I ran the build command, but then I couldn't push. I followed the above tutorials and first thought I was meant to run this:

    # WRONG COMMAND
     docker push docker.io/paulegradie/paulegradie-server

So after a bit of reading I came across:
https://stackoverflow.com/questions/41984399/denied-requested-access-to-the-resource-is-denied-docker

Basically when you create the image name, it needs the username attached if you're gonna push it to docker hub, so
you can basically rename it using the `docker tag` command:

    docker tag paulegradie-server paulegradie/paulegradie-server
    docker push paulegradie/paulegradie-server

Then you can make sure you're logged in:

    docker login

(Then provide your credentials)


#### How to remove a docker image

Docker provides a single command that will clean up any resources — images, containers, volumes, and networks — that are dangling (not associated with a container):

    docker system prune

To additionally remove any stopped containers and all unused images (not just dangling images), add the -a flag to the command:

    docker system prune -a

##### Removing Docker Images
Remove one or more specific images
Use the docker images command with the -a flag to locate the ID of the images you want to remove. This will show you every image, including intermediate image layers. When you’ve located the images you want to delete, you can pass their ID or tag to docker rmi:

List:

    docker images -a

Remove:
    docker rmi Image Image


#### That was ubuntu, how about IIS?

For a container that has an windows server core image with iis and the application installed inside it, we need a slightly different dockerfile.

At the beginning of our dockerfile, we can add:

    FROM microsoft/iis:10.0.14393.206
    SHELL ["powershell"]