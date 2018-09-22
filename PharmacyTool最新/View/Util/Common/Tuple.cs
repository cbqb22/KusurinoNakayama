namespace View.Util.Common
{
    /// <summary>
    /// Tupleクラス
    /// </summary>
    /// <typeparam name="T1">型１</typeparam>
    /// <typeparam name="T2">型２</typeparam>
    public class Tuple<T1, T2> : NotifyPropertyChangedBase
    {
        /// <summary>
        /// Value1
        /// </summary>
        private T1 _Value1;
        /// <summary>
        /// Value1
        /// </summary>
        public T1 Value1 { get { return _Value1; } set { _Value1 = value; FirePropertyChanged("Value1"); } }
        /// <summary>
        /// Value2
        /// </summary>
        private T2 _Value2;
        /// <summary>
        /// Value2
        /// </summary>
        public T2 Value2 { get { return _Value2; } set { _Value2 = value; FirePropertyChanged("Value2"); } }
    }


    /// <summary>
    /// Tupleクラス
    /// </summary>
    /// <typeparam name="T1">型１</typeparam>
    /// <typeparam name="T2">型２</typeparam>
    /// <typeparam name="T3">型3</typeparam>
    public class Tuple<T1, T2, T3> : NotifyPropertyChangedBase
    {
        /// <summary>
        /// Value1
        /// </summary>
        private T1 _Value1;
        /// <summary>
        /// Value1
        /// </summary>
        public T1 Value1 { get { return _Value1; } set { _Value1 = value; FirePropertyChanged("Value1"); } }
        /// <summary>
        /// Value2
        /// </summary>
        private T2 _Value2;
        /// <summary>
        /// Value2
        /// </summary>
        public T2 Value2 { get { return _Value2; } set { _Value2 = value; FirePropertyChanged("Value2"); } }
        /// <summary>
        /// Value2
        /// </summary>
        private T3 _Value3;
        /// <summary>
        /// Value2
        /// </summary>
        public T3 Value3 { get { return _Value3; } set { _Value3 = value; FirePropertyChanged("Value3"); } }
    }


    /// <summary>
    /// Tupleクラス
    /// </summary>
    /// <typeparam name="T1">型１</typeparam>
    /// <typeparam name="T2">型２</typeparam>
    /// <typeparam name="T3">型3</typeparam>
    /// <typeparam name="T4">型4</typeparam>
    public class Tuple<T1, T2, T3, T4> : NotifyPropertyChangedBase
    {
        /// <summary>
        /// Value1
        /// </summary>
        private T1 _Value1;
        /// <summary>
        /// Value1
        /// </summary>
        public T1 Value1 { get { return _Value1; } set { _Value1 = value; FirePropertyChanged("Value1"); } }
        /// <summary>
        /// Value2
        /// </summary>
        private T2 _Value2;
        /// <summary>
        /// Value2
        /// </summary>
        public T2 Value2 { get { return _Value2; } set { _Value2 = value; FirePropertyChanged("Value2"); } }
        /// <summary>
        /// Value3
        /// </summary>
        private T3 _Value3;
        /// <summary>
        /// Value3
        /// </summary>
        public T3 Value3 { get { return _Value3; } set { _Value3 = value; FirePropertyChanged("Value3"); } }
        /// <summary>
        /// Value4
        /// </summary>
        private T4 _Value4;
        /// <summary>
        /// Value2
        /// </summary>
        public T4 Value4 { get { return _Value4; } set { _Value4 = value; FirePropertyChanged("Value4"); } }
    }


    /// <summary>
    /// Tupleクラス
    /// </summary>
    /// <typeparam name="T1">型１</typeparam>
    /// <typeparam name="T2">型２</typeparam>
    /// <typeparam name="T3">型3</typeparam>
    /// <typeparam name="T4">型4</typeparam>
    /// <typeparam name="T5">型5</typeparam>
    public class Tuple<T1, T2, T3, T4, T5> : NotifyPropertyChangedBase
    {
        /// <summary>
        /// Value1
        /// </summary>
        private T1 _Value1;
        /// <summary>
        /// Value1
        /// </summary>
        public T1 Value1 { get { return _Value1; } set { _Value1 = value; FirePropertyChanged("Value1"); } }
        /// <summary>
        /// Value2
        /// </summary>
        private T2 _Value2;
        /// <summary>
        /// Value2
        /// </summary>
        public T2 Value2 { get { return _Value2; } set { _Value2 = value; FirePropertyChanged("Value2"); } }
        /// <summary>
        /// Value3
        /// </summary>
        private T3 _Value3;
        /// <summary>
        /// Value3
        /// </summary>
        public T3 Value3 { get { return _Value3; } set { _Value3 = value; FirePropertyChanged("Value3"); } }
        /// <summary>
        /// Value4
        /// </summary>
        private T4 _Value4;
        /// <summary>
        /// Value2
        /// </summary>
        public T4 Value4 { get { return _Value4; } set { _Value4 = value; FirePropertyChanged("Value4"); } }
        /// <summary>
        /// Value4
        /// </summary>
        private T5 _Value5;
        /// <summary>
        /// Value2
        /// </summary>
        public T5 Value5 { get { return _Value5; } set { _Value5 = value; FirePropertyChanged("Value5"); } }
    }

}
