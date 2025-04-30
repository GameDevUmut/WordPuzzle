Projede örnek olarak Zynga'nın Boggle isimli oyununu aldım, grid içindeki kelime yazma stili benzerdir.
Proje temelinde additive sahne yapısı ve dependency injection framework olarak VContainer kullandım.
Objelerin genel iletişimi, dependency kırılımı ve modülerlik dependency injection patternı sayesinde sağlanıyor.
Örnek olarak 2 adet Unit Test Suite ekledim, bunlar test assembly'ı altında ve Grid ve Trie oluşturma sistemlerini test ediyor.

Bunun dışında Cysharp'dan UniTask ve R3 librarylerini kullandım, UniTask sayesinde düşük overhead ile taskleri unityde kullanıyoruz.
Fakat bazı yerlerde thread parallelization gerektiğinden Task.Run da kullandım, bunu özellikle Trie yapısını buildlerken ve tekli/gridde kelime
aratırken kullandım ki Unity main thread etkilenmesin.
Sözlük veritabanı olarak githubdan TDK'daki Türkçe kelimeleri içeren bir text dosyası buldum, buradaki sözcükleri bir editor tool
yardımı ile JSON formatına çevirip listelettim. JSON dosyası text asset olarak addressable'lardan yüklenmekte ve Newtonsoft JSON ile deserialize edilmekte.
Bu sayede gerek duyulduğu takdirde memoryden kolayca release edilebilir. TDK sözlükteki kelimeleri 3 harf ve üstüyle sınırladım, tek harf ve 2 harfli
kelimeler çok fazla eşleşme yapıyordu.

Observer patternı R3 librarysi ile implement ettim, ReactiveProperty'ler ile UI'ın kolayca dinamik güncellenmesini sağlıyoruz. Oyunu geliştirmek
istediğimiz takdirde bize daha kompleks event triggerları kurmamız için olanak sağlıyor.
Bunun dışında basit animasyonlar/tweenler için DOTween'den faydalandım. 
Art tarafını da biraz olsun görsellik katmak amacıyla Imagen 3 gibi resim oluşturucu generative ai'lar ve hazır UI paketi ile çözdüm.
Farklı aspect ratiolara uygun çalışacak şekilde responsive UI ekledim.

Teşekkür ederim,
Umut Can ERYILDIZ
