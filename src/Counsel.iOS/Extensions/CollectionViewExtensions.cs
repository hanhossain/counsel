namespace UIKit
{
	public static class CollectionViewExtensions
	{
		public static void RegisterClassForCell<T>(this UICollectionView collectionView, string reuseIdentifier) where T : UICollectionViewCell
		{
			collectionView.RegisterClassForCell(typeof(T), reuseIdentifier);
		}
	}
}
