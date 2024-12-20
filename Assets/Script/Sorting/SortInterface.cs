using System.Collections;
using System.Collections.Generic;


public interface ISort{
    //bool은 더이상 sorting을 안해도 되는지 해야하는지 여부를 반환함
    //더 해야하는 경우 false, 끝난 경우 true 반환.
    public IEnumerator UpdateSort();
    public void SetSortList(List<SortObject> sortList);
}
