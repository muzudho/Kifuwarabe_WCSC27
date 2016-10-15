//ProjectDark.NamedInt.Sample :: (C)2014 Project Dark* / Hikaru* All Rights Reserved.

using System;

//Start :: NamedInt�̗��p�錾(using)

//���ȉ���using���g�p����t�@�C�����ׂĂɏ����K�v������܂��B�����܂��A�t�@�C�����m�œ��e���ς��Ȃ��悤�ɂ��Ă��������I����
//using �^�� = ProjectDark.NamedInt.NamedInt0�`9;
using Square = ProjectDark.NamedInt.NamedInt0; //�}�X��
using Piece = ProjectDark.NamedInt.NamedInt1; //�R�}
//�ȏ�

//��StrictNamedIntX�ƁANamedIntX�͕ʕ��ł��I�I
using StrictPiece = ProjectDark.NamedInt.StrictNamedInt1; //�R�}�i�Ö�int�ϊ��Ȃ��j
using StrictMeg = ProjectDark.NamedString.StrictNamedString0;
//End :: NamedInt�̗��p�錾(using)

using ProjectDark.KWSupport;

namespace ProjectDark.NamedInt {
	public class Sample {
		public static void Main() {


			/* TimeoutReader */
			Console.WriteLine("Please input: ");
			string STR = TimeoutReader.ReadLine(5000);
			Console.WriteLine("Time over!: {0}", STR);


			/* NamedInt �̎g���� */

			//�������A���ʂ�int�Ƃ��Ďg���܂��B
			Square S = 1234;
			Piece P = 5678;

			P++; //++, --�ɑΉ����܂����B

			Console.WriteLine("S = {0}, P = {1}", S, P);

			//if�����đ��v�ł�
			if (S == 1234) {
				Console.WriteLine("S��1234�ł��I");
			}

			//�ł��ʕ����m�̑���͂ł��܂���c �����̍s�̃R�����g���O���Ă݂Ă���������
			//S = P; 



			/* StrictNamedInt �̎g���� */
			//�Öق̌^�ϊ����s���Ȃ��ꍇ�ȊO�́ANamedInt�Ɠ����ł����c

			P = 2; //�O�����g���܂킵�ł��A���ʂ�NamedInt�ł��B
			StrictPiece PS = 2; //���ꂪ "Strict"��NamedInt�ł��B

			//�����̎����p�B
			int[] test = new int[16];

			//������͈Öق�int�ɂȂ�܂����c
			test[P] = 1234;

			//Strict�ł͈Öٕϊ�����܂���j�����̍s�̃R�����g���O���Ă݂Ă���������
			//		test[PS] = 1234;

			//�������A�����^�ϊ���OK�ł��B
			test[(int)PS] = 5678;



			/* StrictNamedInt �Ə���int�݂̂��󂯓����z�� */

			//�����ł́Aint[]�̑�����쐬���܂��B
			//�錾: StrictIndexerArray<�z��̌^, �󂯓����StrictNamedInt> = new StrictIndexerArray<�z��̌^>(�T�C�Y, �󂯓����StrictNamedInt);
			
			ProjectDark.NamedInt.StrictIndexerArray<int, StrictPiece> SIA = new ProjectDark.NamedInt.StrictIndexerArray<int, StrictPiece>(16);�@//�v�f��16��int�z��
			SIA[PS] = 5678; //�L���X�g���邱�ƂȂ��A�z��Y�����ɂł��܂��B
			SIA[0] = 1234; //����int�������܂��B

			//SIA[P} = 9012; //�w��ȊO��StrictNamed�͂������ANamedInt�����߂ł��B���R�����g���O���Ă݂Ă���������
			//�������A�R���p�C���G���[���u�^�̈Ⴂ��\�����́v�ł͂Ȃ��u���@���Ԉ���Ă�����́v�Ƃ��ĕ\������Ă��܂��̂ŕ�����ɂ������ł��B

			//Length��A
			System.Console.WriteLine("�z��̒���: {0}", SIA.Length);

			//foreach�����̂܂܎g���܂�
			System.Console.Write("�z��̒��g: ");
			foreach (int Nakami in SIA) {
				System.Console.Write("{0}, ", Nakami);
			}
			System.Console.WriteLine("�ȏ�B");

			//Sort (IComparable)��AIConvertible�����p�ł��܂��B
			System.Collections.Generic.List<StrictPiece> keyList = new System.Collections.Generic.List<StrictPiece>();
			keyList.Add(123);
			keyList.Add(456);
			keyList.Sort();�@//�ꉞ�\�[�g���Ƃ��H

			//�֐��̈�����������Array�����󂯕t���Ȃ��ꍇ�̓L���X�g���Ă��܂���OK�ł��B
			Array A = (Array)SIA;


			//�n�b�V���e�[�u���̃L�[�Ƃ��Ďg���e�X�g
			System.Collections.Hashtable H = new System.Collections.Hashtable();
			H.Add((StrictPiece)1, 1);
			H.Add(1, 1);
			Console.WriteLine("Hashtable: {0}", H.ContainsKey((StrictPiece)1));
			Console.WriteLine("Hashtable: {0}", H.ContainsKey(1));

			//Generics��Dicrionary�Ŏg���Ă݂�
			var Dict = new System.Collections.Generic.Dictionary<StrictPiece, StrictPiece>();
			Dict.Add((StrictPiece)1, (StrictPiece)111);
			Dict.Add((StrictPiece)2, (StrictPiece)12);
			Console.WriteLine("Dictionary: {0}", Dict.ContainsKey( (StrictPiece)1) );

			
			//int�Ƃ�if()
			StrictPiece IFTEST_SI = 1234;
			int IFTEST_INT = 1234;

			if ((int)IFTEST_SI == IFTEST_INT) {
				Console.WriteLine("IfTest: True");
			} else {
				Console.WriteLine("IfTest: False");
			}
			if ((int)IFTEST_SI != IFTEST_INT) {
				Console.WriteLine("IfTest: True");
			} else {
				Console.WriteLine("IfTest: False");
			}

			if (IFTEST_SI == (StrictPiece)IFTEST_INT) {
				Console.WriteLine("IfTest2: True");
			} else {
				Console.WriteLine("IfTest2: False");
			}
			if (IFTEST_SI != (StrictPiece)IFTEST_INT) {
				Console.WriteLine("IfTest2: True");
			} else {
				Console.WriteLine("IfTest2: False");
			}


			//StrictNamedString (��T�|�[�g)
			StrictMeg M = "�e�X�g������";
			Console.WriteLine(M);

	//		Console.WriteLine("{0}", X.Equals(X));

#if BUILD_VS
			System.Console.WriteLine("*: Press Enter to exit.");
			System.Console.ReadLine(); //VS�r���h���AEnter�҂�������i�ȈՁj
#endif

		}
	}
}