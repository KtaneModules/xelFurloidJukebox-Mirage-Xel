using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rnd = UnityEngine.Random;
using KModkit;

public class FurloidJukebox : MonoBehaviour {
    public KMSelectable vinyl;
    public KMSelectable leftArrow;
    public KMSelectable rightArrow;
    public TextMesh questionText;
    public TextMesh answerText;
    public AudioClip[] songSnippets;
    public TextMesh detail;
    string[] songNames = new string[] { "donor song", "Choose me", "Connecting", "Sing a Song", "Corpse Dance", "Accidentally", "papermoon", "Tokyo Teddy Bear", "Lost One’s Weeping", "Grey One", "Eine Klein (Accoustic Arrange Version)", "melancholic", "WAVELIFE", "Two-Faced Lovers", "Phantom Thief F’s Scenario\n~The Mystery of The Missing Diamond~", "Hurting for a Very Hurtful Pain", "Akatsuki Arrival", "An Uncooperative Screw and the Rain", "Lost in Thoughts All Alone", "quiet room", "Music Music", "Anti Beat", "Q", "Headphone Actor", "Blessing" };
    string[] songs = new string[] { "おじかんちょっといいですかちょっとしつもんいいですか時間はとらせないのでどれかに丸を付けてください", "あの日　出会わなきゃ　よかっただなんて古臭い　フレーズを　口にするけれど誰のものでもいいあなたを愛している誰にも渡さない", "誰かの叫ぶ声がする行き場を無くした行き場を無くした名前も顔も分からない君の優しさにどれだけ救われただろう", "レッツ シンガソン さあ　一緒に歌おう街中の灯りの向こうから　聞こえるみんなのハーモニーレッツ シンガソン さあ　一緒に歌おうおもいっきり泣いたあとには　いつもの笑顔を見せておくれよ", "うれしたのしの　しかばね音頭きみも仲間に入れたげる有象に無象の魑魅魍魎　さあ墓場で踊りましょう　チャチャ　ウッ", "深層心理を読み解くような一度のミスが命取りな張り詰め過ぎた　辟易の果てに出会い　accidentally", "けれど 見上げたら 夜空の月の先に 思い出してしまう あの暖かい言葉を", "全然つかめないきみのこと全然しらないうちにこころ奪おうとしてたのはわたしのほうだもん", "ぴぴぴ　聞こえますか今走るよ　風掴むために弾んだ音を生み出して振動の波寄せて愛の形よ舞い上がれ", "前が歪む　涙が邪魔臭くてそれならば笑顔で行こうとまっすぐに決めたのラララほら明日へと　ありがとうどうして尽くめ の毎日 そうしてああしてこうしてサヨナラベイベー現実直視と現実逃避の表裏一体なこの心臓どこかに良いことないかな　なんて裏返しの自分に問うよ　自問自答　自問他答　他問自答連れ回し　ああああ", "10秒ほどで停電が回復 偶然のトラブル銃声はどこから聞こえてきた手荷物検査は通れない窓ガラスが壊れているみたい人が通れるくらい誰かが倒れている    キャー", "何が痛い　何で痛い どうしてこんなにとても痛い 何が痛い　何で痛い どうしてこんなに痛がりたい", "共に走って知って嫉妬して　背中をずっと追っていって並んで　なんだこんなもんか　って笑って先を走ってくっていったって　限度あるってなんて勝手走っても走っても追いつけない忘れない───忘れないから　最高のライバルを", "ねえ　鼓膜　溶ける感覚指の　先で　光る体温僕は　未だ　わからないよ", "あなたは海の灰色の波です", "鮮やかが煩い公園でシーソー穏やかな心が回転しそうだ涙みたいきらきら二人照らす鈴灯", "ミュージックミュージック　このゆびとまれサイレンスサイレンス　さわがないで君への想いを　鳴らすスキマなんてないの　ないの　ないの　ねえ　かき消してよ", "アンチビート命じます オンビート刻みませアンチビート止まらない 制御不能のビートをアンチビートもう早く死んじゃいたい 楽になりたいんだアンチビートでもでも「痛い痛い」 どこにも逝けないんだ僕は", "さあ掻き乱せ衝動のまま今吐き散らす言葉の中きっと嘘しかみつけられないから知ったところでさ（ぱらっぱっぱっぱら）", "その日は随分と平凡で当たり障りない一日だった暇つぶしに聞いてたラジオから", "あの話が流れだすまではよく食べて　よく眠ってよく遊んで　よく学んでよく喋って　よく喧嘩してごく普通な毎日を泣けなくても　笑えなくても歌えなくても　何もなくても愛せなくても　愛されなくてもそれでも生きて欲しい" };
    int songIndex;
    int displayIndex;
    public KMBombModule module;
    public KMAudio sound;
    int moduleId;
    static int moduleIdCounter = 1;
    bool solved;

	void Awake () {
        moduleId = moduleIdCounter++;
        leftArrow.OnInteract += PressArrow(false);
        rightArrow.OnInteract += PressArrow(true);
        vinyl.OnInteract += SumbitAnswer();
        ResetModule();
    }

    void ResetModule()
    {
        songIndex = rnd.Range(0, 25);
        displayIndex = rnd.Range(0, 25);
        questionText.text = songs[songIndex].Substring(rnd.Range(0, songs[songIndex].Length - 6), 5);
        answerText.text = songNames[displayIndex];
        Debug.LogFormat("[The Furloid Jukebox #{0}] The song that was chosen is {1}.", moduleId, songNames[songIndex]);
        Debug.LogFormat("[The Furloid Jukebox #{0}] The text shown on the module is {1}.", moduleId, questionText.text);
    }
    KMSelectable.OnInteractHandler PressArrow (bool arrow) {
        return delegate
        {
            if (!solved)
            {
                sound.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
                if (arrow)
                {
                    rightArrow.AddInteractionPunch();
                    displayIndex++;
                    if (displayIndex == 25) displayIndex = 0;
                }
                else
                {
                    leftArrow.AddInteractionPunch();
                    displayIndex--;
                    if (displayIndex == -1) displayIndex = 24;
                }
                answerText.text = songNames[displayIndex];
            }
            return false;
        };
	}
    KMSelectable.OnInteractHandler SumbitAnswer()
    {
        return delegate
        {
            if (!solved)
            {
                sound.PlaySoundAtTransform("recordScratch", transform);
                vinyl.AddInteractionPunch();
                Debug.LogFormat("[The Furloid Jukebox #{0}] You submitted {1}.", moduleId, songNames[displayIndex]);
                if (displayIndex == songIndex) {
                    Debug.LogFormat("[The Furloid Jukebox #{0}] That was correct. Module solved.", moduleId);
                    sound.PlaySoundAtTransform(songSnippets[songIndex].name, transform);
                    module.HandlePass();
                    detail.text = "UwU";
                    solved = true;
                }
                else
                {
                    Debug.LogFormat("[The Furloid Jukebox #{0}] That was incorrect. Strike!", moduleId);
                    module.HandleStrike();
                    ResetModule();
                }
            }
            return false;
        };
    }
}
