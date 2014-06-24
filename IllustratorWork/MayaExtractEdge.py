import maya.OpenMaya as OM
list = OM.MSelectionList()
OM.MGlobal_getActiveSelectionList(list)
for i in range(0,list.length()):
       obj= OM.MObject()
       dpath= OM.MDagPath()
       list.getDagPath(i,dpath,obj)
       meshFn=OM.MFnMesh(dpath)
       vts=OM.MPointArray()
       meshFn.getPoints(vts)
       for j in range(0,vts.length()):
         # print ("The object "+dpath.partialPathName()+ "'s vertex #"+ str(j+1)+" is :")
          print (str(vts[j].x) + ", " + str(vts[j].y) + ", " + str(vts[j].z) + ", ")