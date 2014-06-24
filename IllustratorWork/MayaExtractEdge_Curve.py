import maya.OpenMaya as OM
import maya.cmds as mc
list = OM.MSelectionList()
OM.MGlobal_getActiveSelectionList(list)
for i in range(0,list.length()):
    obj= OM.MObject()
    dpath= OM.MDagPath()
    list.getDagPath(i,dpath,obj)
    curveFn=OM.MFnNurbsCurve()
    #curveFn=OM.MObject()
    curveFn.setObject(dpath)
    cvArray= OM.MPointArray()
    curveFn.getCVs(cvArray, OM.MSpace.kWorld)
    #cvIter= OM.MItCurveCV(curveFn)
    
    #cvs= OM.MPointArray()

    #curveFn.getCvs(cvs);
    
    #MItCurveCV* cvIter = new MItCurveCV( curve, &status);
    #cvIter = OM.MItCurveCV(curveFn, &status)

    #meshFn=OM.MFnMesh(dpath)
    #vts=OM.MPointArray()
    #meshFn.getPoints(vts)
    #print curveFn.numCVs()
    #for j in range(0,curveFn.numCVs()):
    for j in range(0,cvArray.length()):
        #print cvArray.operator(j)
        print cvArray[j][0], cvArray[j][1]
        #print mc.pointPosition(curveFn.cv(j))
        #print (str(vts[j].x) + ", " + str(vts[j].y) + ", " + str(vts[j].z) + ", ")
        # print ("The object "+dpath.partialPathName()+ "'s vertex #"+ str(j+1)+" is :")
