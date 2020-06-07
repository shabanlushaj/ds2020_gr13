
## DATASECURITY - project




#### Compile and excecute:

Check if .NET Core is installed on your OS.

```$ dotnet --info```

if not:

Install the [.NET Core].

[.NET CORE]: https://dotnet.microsoft.com/download


Clone the repository or direct download from [Github].

[Github]:https://github.com/shabanlushaj/ds2020_gr13

```$ git clone https://github.com/shabanlushaj/ds2020_gr13```


Go to folder 'ds':

```$ cd ./ds/```


Run program:

```$ dotnet run```


OR


If you have VISUAL STUDIO IDE installed, you can simply open ```ds.sln``` and run.

This procedure installs the program.


#### To excecute the program:
The installation procedure generates new folders and files.

Go to 'bin/debbug/netcoreapp3.1' and open terminal or

```$ cd ./bin/Debug/netcoreapp3.1/```


Now excecute the commands:

create an alias to call it simplier:

```$ alias ds=./ds.exe```

### 1ST TASK  

1.1. Added password and storage saved in db

##### ```<create-user> <user-name>```

```
$ ds create-user edon
Jepni fjalekalimin:Fiek2020.
Perserite fjalekalimin:Fiek2020.
Eshte krijuar shfrytezuesi 'edon'
Eshte krijuar celsi privat: 'keys/edon.xml'
Eshte krijuar celsi publik: 'keys/edon.pub.xml'
```

2.1. Deletes also db records

##### ```<delete-user> <user-name>```

```
$ ds delete-user edon
Eshte larguar celsi privat: 'keys/edon.xml'
Eshte larguar celsi publik: 'keys/edon.pub.xml'
Eshte fshire shfrytezuesi 'edon'
```

3. Authenticate user, issue a token

##### ``` <login> <user-name>```

```
$ ds login edon
Jepni fjalekalimin:Fiek2020.
Token: eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyIgVXNlciAiOiJlZG9uIiwiIFNrYWRpbWkgIjoiMjAyMC0wNi0wNyAwNDozMSBhbSJ9.8jKn6DaqjwkI4iM5OAZ03dlcijm1w-ODTx5LKahPwAg

```

4. Shows the status of the token

##### ``` <status> <token>```

```
$ ds status eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyIgVXNlciAiOiJlZG9uIiwiIFNrYWRpbWkgIjoiMjAyMC0wNi0wNyAwNDozMSBhbSJ9.8jKn6DaqjwkI4iM5OAZ03dlcijm1w-ODTx5LKahPwAg

" User ":"edon"
" Skadimi ":"2020-06-07 04:31 am"
```

5. Extended 3rd method from the 2nd task :  Added -> Signature message, if token is valid

##### ```<write-message> <user-name> <text> <token>```

```
$ ds write-message blerim "takohemi neser" eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyIgVXNlciAiOiJlZG9uIiwiIFNrYWRpbWkgIjoiMjAyMC0wNi0wNyAwNDozMSBhbSJ9.8jKn6DaqjwkI4iM5OAZ03dlcijm1w-ODTx5LKahPwAg

Ymxlcmlt.VGVlRWJPeFlrYUE9.Gdo0Ut3o1cSDQfQaLL3SgLbUuV39f/i6c68whUYnPWS4o0RDyTI2ivfhIGnN71F+NLRH11v5TkXxm09/LrGc4m3bnQPDRS0xfvp7LqGr2Tiz5RqEJgoJceSqy3R9Zr3/cZMgErua65UZOl0tKWRzZejqDRXoZXHNF4gD3ab7GKQ=.QYzMeyTtugPvGYXA2pz6iw==.ZWRvbg==.ZXRrUWRjVXpqbXppazdObkwxaDlSK3psdVVPY2VxZTNPQnhwR21yQ25Yd3FKdHhkc0ljM0Q1Q2ZFRjdTMUw1anJhcXJQRGM5SjJWWkJDUHlEQi9TOHlnZklBZzZqU0hPOHpOdDJEZ21YVjNhTjV0amJHeC9EU3Z0bGNYTEtzVFhBeTZFYXNCalVGbFF2b2luMGJaWWwwZDh6M1MxMm5Vb3ordzllSmhBR1BVPQ==
```

6. Extended 4th method from the 2nd task : Added -> Checks if signature is valid

##### ```<read-message> <cipher>```

```
$ ds read-message Ymxlcmlt.VGVlRWJPeFlrYUE9.Gdo0Ut3o1cSDQfQaLL3SgLbUuV39f/i6c68whUYnPWS4o0RDyTI2ivfhIGnN71F+NLRH11v5TkXxm09/LrGc4m3bnQPDRS0xfvp7LqGr2Tiz5RqEJgoJceSqy3R9Zr3/cZMgErua65UZOl0tKWRzZejqDRXoZXHNF4gD3ab7GKQ=.QYzMeyTtugPvGYXA2pz6iw==.ZWRvbg==.ZXRrUWRjVXpqbXppazdObkwxaDlSK3psdVVPY2VxZTNPQnhwR21yQ25Yd3FKdHhkc0ljM0Q1Q2ZFRjdTMUw1anJhcXJQRGM5SjJWWkJDUHlEQi9TOHlnZklBZzZqU0hPOHpOdDJEZ21YVjNhTjV0amJHeC9EU3Z0bGNYTEtzVFhBeTZFYXNCalVGbFF2b2luMGJaWWwwZDh6M1MxMm5Vb3ordzllSmhBR1BVPQ==
Marresi: blerim
Mesazhi: takohemi neser
Derguesi: edon
Validimi: Nenshkrimi eshte valid.
```

### 2ND TASK  

1. Generates a pair of private and public keys. XML-format

##### ```<create-user> <user-name>```

```
$ ds create-user edon
Eshte krijuar celsi privat: 'keys/edon.xml'
Eshte krijuar celsi publik: 'keys/edon.pub.xml'

$ ds create-user edon
Gabim: Celesi 'edon' ekziston paraprakisht.
```


2. Deletes user's pair of private and public keys.

##### ```<delete-user> <user-name>```

```
$ ds delete-user edon
Eshte larguar celsi privat: 'keys/edon.xml'
Eshte larguar celsi publik: 'keys/edon.pub.xml'

$ ds delete-user edon
Gabim: Celesi 'edon' nuk ekziston.
```

3. Encrypts a message with users public key.

##### ```<write-message> <user-name> <text> || [file]```

```
$ ds write-message edon "takimi mbahet te shtunen ne ora 18:00"
ZWRvbg==.aUZsRUw1ZWVzOGM9.hxuqnLr1pZz43JokZu0UTfeM7aFOVH/yUVOoz9ksTWXBodWJko2YDhiHvKRCSTvpPumKqZU7lUJI98BzV6ZlM8azC2TaHp1lRpM4OBm0vUtyxzSjoELKePJcnCqGPRpK4lM6IHHd8BgaOm1UJFPwKF47Meu09PEBkfDmPHIWeVg=.taWPUz0ks2LWTc6RrRBesYDiTxGymgti1bMxuZTWZB5/RxxFz9Q+qQ==

$ ds write-message edon "takimi mbahet te shtunen ne ora 18:00" edon.txt
Mesazhi i enkriptuar u ruajt ne fajllin: files/edon.txt
```


4. Decrypts a message with users private key.

##### ```<read-message> <cipher>```

```
$ ds read-message ZWRvbg==.aUZsRUw1ZWVzOGM9.hxuqnLr1pZz43JokZu0UTfeM7aFOVH/yUVOoz9ksTWXBodWJko2YDhiHvKRCSTvpPumKqZU7lUJI98BzV6ZlM8azC2TaHp1lRpM4OBm0vUtyxzSjoELKePJcnCqGPRpK4lM6IHHd8BgaOm1UJFPwKF47Meu09PEBkfDmPHIWeVg=.taWPUz0ks2LWTc6RrRBesYDiTxGymgti1bMxuZTWZB5/RxxFz9Q+qQ==
Marresi: edon
Dekriptimi: takimi mbahet te shtunen ne ora 18:00

$ ds read-message "C:\Users\Admin\Desktop\SIGURIA_DHENAVE\ds\files\edon.txt"
Marresi: edon
Dekriptimi: takohemi neser
```

5. Export-key

##### ``` export-key <public-private> <name> || [file]```

```
$ ds export-key private edon
<RSAKeyValue>
<Modulus>
ukC4v8v7Sc0/3wvCNwsHj0tt5KsbdWhXEIZrcl1ciJ79ugcNRHl1jRrdq1Tw+vDQtEidEu4fqNE/SpY48k6cv58EPNd1uPaoFs0VOPfam2+0x0mCKor+gOoy2ayuQIJVaQGP26BuhHF4RkS2g8igcfekBTLTEWN6Pik2hmzlZx0=</Modulus>
<Exponent>
AQAB</Exponent>
<P>
5fXBaUToc43F6CXgQZQ+jAKQ+fVjR7dkLgHSKqeT02GNBXNd6Pu3MRxks215OqA02P2eW3TE+4hrjz+IHenNow==</P>
<Q>
z1fzntPC4l1fpdlY1olQQLmEgv/OV7M3LyHWeHxyyP6yl4z4w/bsXzgjYtt4Vy5U8tRBzzxqDm2AY31kvhnEPw==</Q>
<DP>
hZiixr+LtCY3RclLYY34UGrlQvI2vWFjx/6y1KkKjpFr1jDR7BrgsJ1oO31sIo0UZsPhDgzmq6LzqgMk1wwPtQ==</DP>
<DQ>
jofcPZM+RZOAW6bULe4Ij+W3lMG6G4lj5u9w2jRaR5bmN829eCB2jmIt2RgbWnrMBH0qnAXgtfhKKJobNZ7Kyw==</DQ>
<InverseQ>
LqmhYJOM81EvoxVNKZjTprOIpCyVeqZNwlG+zTC73DuA0IEuvqpbN25RBp/oHihM/FBFyM7deVXktk/Zgsu3Bw==</InverseQ>
<D>
T+99GxvRkoAbfH/Qb801BmPqGyzwkPgh/b6jGKdHEn+iB5gFMrEqKVAxeNnwvtJh0C4l87ztK4f6sOk+MYhZrwxCQZFWr4Wf8a4QpAuDWIwNTkmq6x44EFn0/ifJAkZWbNhy3C2fqgkWTQYiEpGijxFXlHskB8BBkQM+NxE+S0U=</D>
</RSAKeyValue>
```

```
$ ds export-key public edon edonExp
Celesi publik u ruajt ne fajllin edonExp.pub.xml
```

6. Import-key

##### ``` import-key <name> <path>```

```
$ ds import-key imported  C:\Users\Admin\Desktop\toImport.xml
Celesi privat u ruajt ne fajllin ../../../keys/imported.xml
Celesi publik u ruajt ne fajllin ../../../keys/imported.pub.xml
```

```
$ ds import-key httpimport https://pastebin.com/raw/nvSFtXAd
Celesi privat u ruajt ne fajllin ../../../keys/httpimport.xml
Celesi publik u ruajt ne fajllin ../../../keys/httpimport.pub.xml
```

7. List - keys //shtese

##### ```$ ds list-keys```


### 1ST TASK


### KOMANDA FOUR SQUARE;

#### Nenkomanda encrypt;
``` 
    C:\Users\Admin\Desktop\MOST_IMPORTANT\ds\bin\Debug\netcoreapp3.1>ds four-square encrypt "takohemi neser" siguria dhenave
    Encryption: nndoeelbmetepw
 ```

#### Nenkomanda decrypt
``` 
    C:\Users\Admin\Desktop\MOST_IMPORTANT\ds\bin\Debug\netcoreapp3.1>ds four-square decrypt "nndoeelbmetepw" siguria dhenave
    Decryption: takohemineserx
```

### KOMANDA CASE;

#### LOWER;
``` 
    C:\Users\Admin\Desktop\MOST_IMPORTANT\ds\bin\Debug\netcoreapp3.1>ds case lower "Pershendetje nga Fiek"
    pershendetje nga fiek
```

#### UPPER;
``` 
    C:\Users\Admin\Desktop\MOST_IMPORTANT\ds\bin\Debug\netcoreapp3.1>ds case upper "Pershendetje nga Fiek"
    PERSHENDETJE NGA FIEK
```

#### ALTERNATING;
``` 
    C:\Users\Admin\Desktop\MOST_IMPORTANT\ds\bin\Debug\netcoreapp3.1>ds case alternating "Pershendetje nga Fiek"
    pErShEnDeTjE NgA FiEk
```

#### INVERSE;
``` 
    C:\Users\Admin\Desktop\MOST_IMPORTANT\ds\bin\Debug\netcoreapp3.1>ds case inverse "Pershendetje nga Fiek"
    pERSHENDETJE NGA fIEK
```

#### CAPITALIZE
``` 
    C:\Users\Admin\Desktop\MOST_IMPORTANT\ds\bin\Debug\netcoreapp3.1>ds case capitalize "Pershendetje nga Fiek"
    Pershendetje Nga Fiek
```

#### SENTENCE;
``` 
    C:\Users\Admin\Desktop\MOST_IMPORTANT\ds\bin\Debug\netcoreapp3.1>ds case sentence "Pershendetje nga Fiek"
    pershendetje nga fiek, PershenDetje nga fiek. PERSHENDETJE NGA FIEK! peRshenDetjE nga fiek.
    Pershendetje nga fiek, pershendetje nga fiek. Pershendetje nga fiek! Pershendetje nga fiek.
```
### KOMANDA RAIL-FENCE

#### ENCRYPT
``` 
    C:\Users\Admin\Desktop\MOST_IMPORTANT\ds\bin\Debug\netcoreapp3.1>ds rail-fence encrypt "takohemi neser" 3
   tomneahierke s
```

##### from internet : stackoverflow.com informit.com etc


